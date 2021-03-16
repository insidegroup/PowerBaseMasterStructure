<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.IATAVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - IATA
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">IATA</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>
        <% Html.EnableUnobtrusiveJavaScript(); %>
        <%using (Html.BeginForm(null, null, FormMethod.Post, new { id = "form0" })){%>
        <%= Html.AntiForgeryToken() %>
        
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Create IATA</th> 
		        </tr> 
				<tr>
                    <td><label for="IATA_IATANumber">IATA</label></td>
                    <td><%= Html.TextBoxFor(model => model.IATA.IATANumber, new { maxlength = "8" })%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.IATA.IATANumber)%></td>
                </tr>
				<tr>
                    <td><label for="IATA_IATABranchOrGLString">Branch or GL String</label></td>
                    <td><%= Html.TextBoxFor(model => model.IATA.IATABranchOrGLString, new { maxlength = "50", @Class = "IATABranchOrGLString" })%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.IATA.IATABranchOrGLString)%></td>
                </tr>				
                <tr>
                    <td width="20%" class="row_footer_left"></td>
                    <td width="60%" class="row_footer_centre"></td>
                    <td width="20%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>                    
                    <td class="row_footer_blank_right"><input type="submit" value="Create IATA" title="Create IATA" class="red"/></td>
                </tr>
            </table>
		<% } %>
        </div>
    </div>
<script src="<%=Url.Content("~/Scripts/ERD/IATA.js")%>" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
GDS Management &gt;
<%=Html.RouteLink("IATA", "Main", new { controller = "IATA", action = "ListUnDeleted", }, new { title = "IATA" })%> &gt;
Create
</asp:Content>