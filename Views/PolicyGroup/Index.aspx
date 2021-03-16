<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<CWTDesktopDatabase.Models.PolicyGroup>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Index</h2>

    <table>
        <tr>
            <th></th>
            <th>
                PolicyGroupId
            </th>
            <th>
                Policy Group Name
            </th>
            <th>
                Enabled Flag
            </th>
            <th>
                Enabled Date
            </th>
            <th>
                Expiry Date
            </th>
            <th>
                Inherit From Parent Flag
            </th>
            <th>
                Trip Type ID
            </th>
            <th>
                Deleted Flag
            </th>
            <th>
                Deleted Date/Time
            </th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td>
                <%= Html.ActionLink("Edit", "Edit", new { id=item.PolicyGroupId }) %> |
                <%= Html.ActionLink("Details", "Details", new { id=item.PolicyGroupId })%> |
                <%= Html.ActionLink("Delete", "Delete", new { id=item.PolicyGroupId })%>
            </td>
            <td>
                <%= Html.Encode(item.PolicyGroupId) %>
            </td>
            <td>
                <%= Html.Encode(item.PolicyGroupName) %>
            </td>
            <td>
                <%= Html.Encode(item.EnabledFlag) %>
            </td>
            <td>
                <%= Html.Encode(String.Format("{0:g}", item.EnabledDate)) %>
            </td>
            <td>
                <%= Html.Encode(String.Format("{0:g}", item.ExpiryDate)) %>
            </td>
            <td>
                <%= Html.Encode(item.InheritFromParentFlag) %>
            </td>
            <td>
                <%= Html.Encode(item.TripTypeId) %>
            </td>
            <td>
                <%= Html.Encode(item.DeletedFlag) %>
            </td>
            <td>
                <%= Html.Encode(String.Format("{0:g}", item.DeletedDateTime)) %>
            </td>
        </tr>
    
    <% } %>

    </table>

    <p>
        <%= Html.ActionLink("Create New", "Create") %>
    </p>

</asp:Content>

