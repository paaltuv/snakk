﻿namespace Snakk.API.PluginFramework.Hooks.Routes.Comment.Services
{
    public interface IGet : IPlugin
    {
        void Before(
            long commentId,
            Dto.Routes.Comment.Get.ResponseDto responseDto);

        void After(
            long commentId,
            Dto.Routes.Comment.Get.ResponseDto responseDto);

        void CommentQueryWhereBuilder(LinqKit.ExpressionStarter<DB.Comment> wherePredicate);

        void CommentQuerySelectorBuilder(
            DB.Comment entity,
            dynamic result);
    }
}
