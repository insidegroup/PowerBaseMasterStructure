<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<System.Decimal?>" %>
<%=Html.TextBox("", (Model.HasValue ? Model.Value.ToString("N2") : string.Empty), new { @maxlength = "11" })%>