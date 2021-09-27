﻿//  SPDX-FileCopyrightText: 2021 Pål Rune Sørensen Tuv <me@paaltuv.no>
//  SPDX-License-Identifier: MIT

using Snakk.API.Helpers;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Snakk.API.Routes.Comment.Services
{
    public interface IGet
    {
        Task<Dto.Routes.Comment.Get.ResponseDto> RunAsync(
            long commentId,
            object pluginData);
    }

    public class Get : IGet
    {
        private readonly IEnumerable<PluginFramework.Hooks.Routes.Comment.Services.IGet> _pluginEnumerable;
        private readonly QueryFactory _db;

        public Get(
            IEnumerable<PluginFramework.Hooks.Routes.Comment.Services.IGet> pluginEnumerable,
            QueryFactory db)
        {
            _pluginEnumerable = pluginEnumerable;
            _db = db;
        }

        public async Task<Dto.Routes.Comment.Get.ResponseDto> RunAsync(
            long commentId,
            object pluginData)
        {
            var responseDto = new Dto.Routes.Comment.Get.ResponseDto();

            HookBefore(_pluginEnumerable, commentId, responseDto);

            var comment = await GetComment(
                _pluginEnumerable,
                commentId);

            responseDto.Text = comment.Text;
            responseDto.PluginData = comment.PluginData;

            HookAfter(_pluginEnumerable, commentId, responseDto);

            return responseDto;
        }

        public async Task<(string Text, dynamic PluginData)> GetComment(
            IEnumerable<PluginFramework.Hooks.Routes.Comment.Services.IGet> pluginEnumerable,
            long commentId)
        {
            var commentQuery = _db
                .Query("Comment")
                .Where("Id", commentId)
                .Select("Id", "Text", "CreatedUtc");

            HookCommentQueryBuilderBefore(_pluginEnumerable, commentId, commentQuery);

            var comment = await commentQuery.FirstOrDefaultAsync<QueryResult.Dto.Routes.Comment.Services.Get.CommentDto>();

            HookCommentQueryBuilderAfter(_pluginEnumerable, commentId, comment);

            return (comment.Text, comment.PluginData);
        }

        #region Hook definitions
        private static void HookBefore(
            IEnumerable<PluginFramework.Hooks.Routes.Comment.Services.IGet> pluginEnumerable,
            long commentId,
            Dto.Routes.Comment.Get.ResponseDto responseDto)
            => Hook.Invoke(
                pluginEnumerable,
                i => i.Before(
                    commentId,
                    responseDto));

        private static void HookAfter(
            IEnumerable<PluginFramework.Hooks.Routes.Comment.Services.IGet> pluginEnumerable,
            long commentId,
            Dto.Routes.Comment.Get.ResponseDto responseDto)
            => Hook.Invoke(
                pluginEnumerable,
                i => i.After(
                    commentId,
                    responseDto));

        private static void HookCommentQueryBuilderBefore(
            IEnumerable<PluginFramework.Hooks.Routes.Comment.Services.IGet> pluginEnumerable,
            long commentId,
            SqlKata.Query commentQuery)
            => Hook.Invoke(
                pluginEnumerable,
                i => i.CommentQueryBuilderBefore(commentId, commentQuery));

        private static void HookCommentQueryBuilderAfter(
            IEnumerable<PluginFramework.Hooks.Routes.Comment.Services.IGet> pluginEnumerable,
            long commentId,
            QueryResult.Dto.Routes.Comment.Services.Get.CommentDto commentQueryResultDto)
            => Hook.Invoke(
                pluginEnumerable,
                i => i.CommentQueryBuilderAfter(commentId, commentQueryResultDto));
        #endregion
    }

    public class SelectBuilder<TSource>
    {
        private readonly List<MemberInfo> members = new List<MemberInfo>();

        public SelectBuilder<TSource> Add<TValue>(Expression<Func<TSource, TValue>> selector)
        {
            var member = ((MemberExpression)selector.Body).Member;

            members.Add(member);

            return this;
        }

        public IQueryable<TResult> Select<TResult>(IQueryable<TSource> source)
        {
            var sourceType = typeof(TSource);
            var resultType = typeof(TResult);

            var parameter = Expression.Parameter(sourceType, "e");

            var bindings = members.Select(member => Expression.Bind(
                resultType.GetProperty(member.Name), Expression.MakeMemberAccess(parameter, member)));

            var body = Expression.MemberInit(Expression.New(resultType), bindings);

            var selector = Expression.Lambda<Func<TSource, TResult>>(body, parameter);

            return source.Select(selector);
        }
    }
}
