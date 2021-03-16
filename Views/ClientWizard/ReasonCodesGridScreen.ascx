<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CWTDesktopDatabase.ViewModels.ProductReasonCodeTypesVM>" %>

<table class="" cellspacing="0" width="100%">
        <thead>
        	<tr>
            
            
             <th><span id="clientReasonCodesEditCancelButton"><small>Cancel changes</small></span></th>
                <th><div style="float:right;"><span id="clientReasonCodesEditBackButton"><small><< Back</small></span>
        &nbsp;<span id="clientReasonCodesEditNextButton"><small>Next >></small> </span></div> </th>
                </tr>
                </thead>
        </table>
<table width="100%" id="reasonCodesGridTable" class="tablesorter_other3" cellspacing="0">
    <tr>
        <td style="background-color:#007886;border: 0;font-size: 10pt;padding: 4px;color:#000000;color:#FFF;"><!--empty top left--></td>
        <%
        foreach (var rct in Model.ReasonCodeTypes)
        {
             Response.Write("<td style=\"background-color:#007886;border: 0;font-size: 10pt;padding: 4px;color:#000000;color:#FFF; border-bottom: solid 1px #333; border-right: solid 1px #333;\">" + rct.ReasonCodeTypeDescription + "</td>");
        }
        %>
    </tr>
<%       

          
//build a  column of Products
foreach (var p in Model.Products) 
{
%>
    <tr>
        <td style="background-color:#007886;border: 0;font-size: 10pt;padding: 4px;color:#000000;color:#FFF; border-bottom: solid 1px #333; border-right: solid 1px #333;"><%=p.ProductName %></td>
        <%
        //flag to indicate that item has either default or custom ReasonCodes
        var hasReasonCodes = false; 
        
        //build a row of ReasonCodeTypes
        foreach (var r in Model.ReasonCodeTypes)
        {
            %>
            <td id="<%=p.ProductId%>_<%=r.ReasonCodeTypeId%>" style="border-bottom: solid 1px #333; border-right: solid 1px #333;" onmouseover="this.style.backgroundColor='#BAE899';" onmouseout="this.style.backgroundColor='#FFFFFF';"><%
                 
            //loop ProcudtReasonCodeTypes to find matching ProductId and ReasonCodeTypeId
            foreach (var prct in Model.ProductReasonCodeTypes.ProductReasonCodeType)
            {
                if (prct.Product.ProductId == p.ProductId && prct.ReasonCodeType.ReasonCodeTypeId == r.ReasonCodeTypeId)
                {
                    //CUSTOM or DEFAULT >>>>> if match, loop and show all ReasonCodeGroup Information
                    if (prct.ReasonCodeGroup.Count() > 0)
                    {
                        
                        for (var g = 0; g < prct.ReasonCodeGroup.Count(); g++)
                        {
                            string source = "";
                                                      
                          //  Response.Write("ReasonCodeGroupId[" + g + "]=" + prct.ReasonCodeGroup[g].ReasonCodeGroupId + "<br/>");
                        Response.Write("<h4>" + prct.ReasonCodeGroup[g].Source + "</h4><br/>");
                            source = prct.ReasonCodeGroup[g].Source;
                         %><br />
						
						 <%
                             %>
                             <a href="javascript:EditProductReasonCodeType('<%=prct.ReasonCodeGroup[g].ReasonCodeGroupId.ToString() %>','<%=p.ProductId %>','<%=prct.ReasonCodeType.ReasonCodeTypeId %>','<%=p.ProductName %>','<%=prct.ReasonCodeType.ReasonCodeTypeDescription%>','<% =source%>','<% =g%>')"><img src='../../images/Common/editButton.jpg' border='0' alt="edit"/></a><br/><%
                        }
                        hasReasonCodes = true;
                    }
                    else //DEFAULT(INHERITED)
                    {
                         %> <h4>Default (no content)</h4><br />
                     <br />
	                    
                     <a href="javascript:EditProductReasonCodeType('','<%=p.ProductId%>','<%=prct.ReasonCodeType.ReasonCodeTypeId%>','<%=p.ProductName%>','<%=prct.ReasonCodeType.ReasonCodeTypeDescription%>','default')"><img src='../../images/Common/editButton.jpg' border='0' alt="edit"/></a><br/><%
                    }
                    hasReasonCodes = true;
                }
            }
            if (!hasReasonCodes)
            {
            %><%
            }
                 
        %><span id="<%=p.ProductId%>_<%=r.ReasonCodeTypeId%>_editSpan"></span></td><%
    }
%>
    </tr>
    <%
}
    

 %>

 </table>
  