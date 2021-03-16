
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<System.DateTime?>" %>
<%=Html.TextBox("", (Model.HasValue ? Model.Value.ToString("MMM dd yyyy") : string.Empty)) %>