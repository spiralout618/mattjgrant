using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Collections;
using System.Linq;

namespace mattjgrant.Helpers
{
    public static class HtmlExtensions
    {
        //public static MvcHtmlString EditorForWithTemplate<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, IEnumerable<TValue>>> expression){
        //    //var valueHtmlHelper = new HtmlHelper<TValue>(html.ViewContext, html.ViewDataContainer);
        //    //var template = valueHtmlHelper.Editor(Activator.CreateInstance<TValue>()).ToString();
        //    Func<TModel, IEnumerable<TValue>> function = expression.Compile();
        //    List<TValue> templateList = new List<TValue>();
        //    templateList.Add(Activator.CreateInstance<TValue>());
        //    Expression<Func<TModel, IEnumerable<TValue>>> newExpression = m => templateList.Concat(function(m));
        //    return new MvcHtmlString(html.EditorFor(expression).ToString());
        //}
        //public static MvcHtmlString EditorForWithTemplate<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        //{
        //    if(TValue IEnumerable)
        //    var collectionType = 
        //    //var valueHtmlHelper = new HtmlHelper<TValue>(html.ViewContext, html.ViewDataContainer);
        //    //var template = valueHtmlHelper.Editor(Activator.CreateInstance<TValue>()).ToString();
        //    Func<TModel, IEnumerable<TValue>> function = expression.Compile();
        //    List<TValue> templateList = new List<TValue>();
        //    templateList.Add(Activator.CreateInstance<TValue>());
        //    Expression<Func<TModel, IEnumerable<TValue>>> newExpression = m => templateList.Concat(function(m));
        //    return new MvcHtmlString(html.EditorFor(expression).ToString());
        //}
    }
}