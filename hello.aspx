<%@ Page Language="C#" AutoEventWireup="true" CodeFile="hello.aspx.cs" Inherits="hello" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
     <script type="text/javascript" src="http://code.jquery.com/jquery-1.8.2.js"></script>
       <script type="text/javascript">
           $(function ss(ds) {
               
               alert($("#hfServerValue").val());
              var tables = [ds];
                
               for (var i = 0; i < tables.length; i++) {
                   var content = "<table class='table table-bordered'><thead><tr>";
                   for (var k = 0; k < 6; k++) {
                       content += '<th id="header" style="color:black; font-size: 12px; border:1px solid black; text-align:center;border-color: rgb(166, 166, 166);">' + tables[k] + ' </th>';
                   }
                   content += "</tr></thead></table><br/>";
                   $('#gvEmployee').append(content);
               }



           });
           //function ss() {
           //$(function ss(ds) {
           //    $("#foo").append("<div id='myDiv'>hello world</div>")

           //    var table = $('<table></table>').addClass('foo');

           //    for (i = 0; i < 3; i++) {
           //        var row = $('<tr></tr>').addClass('bar').text('result ' + i);
           //        table.append(row);
           //    }

           //    $('#gvEmployee').append(table);
           //        //var table = document.createElement('table');
           //        //for (var i = 1; i < 4; i++) {
           //        //    //table = table + i;
           //        //    var tr = document.createElement('tr');

           //        //    var td1 = document.createElement('td');
           //        //    var td2 = document.createElement('td');

           //        //    var text1 = document.createTextNode('Text');
           //        //    var text2 = document.createTextNode('Text');

           //        //    td1.appendChild(text1);
           //        //    td2.appendChild(text2);
           //        //    tr.appendChild(td1);
           //        //    tr.appendChild(td2);
           //        //    table.appendChild(tr);
           //        //}

           //        //document.body.appendChild(table);
           //});
        //}
           </script>
<body>
    <form id="form1" runat="server">
    <div runat="server"> 
        <asp:HiddenField ID="hfServerValue" runat="server" />      
    <asp:GridView ID="gvEmployee" runat="server" AutoGenerateColumns="false">
    <Columns>
        <asp:BoundField DataField="TaxType" HeaderText="TaxType" ItemStyle-Width="150px" />
        <asp:BoundField DataField="TaxYear" HeaderText="TaxYear" ItemStyle-Width="100px" />
        <asp:BoundField DataField="ParcelId" HeaderText="ParcelId" ItemStyle-Width="100px" />
    </Columns>
</asp:GridView>

    </div>
    </form>
</body>
</html>
