// JScript File
   	function SelectRow()
    	{
    		var obj = window.event.srcElement ? window.event.srcElement : window.event.target;
    		if(obj.tagName=="INPUT")    //this is a checkbox
    		{
    		    checkRowOfObject(obj);
    		}
    		else if (obj.tagName=="TD") //this a table cell
    		{
    		    //get a pointer to the tablerow
    		    var row = obj.parentNode;
    		    var chk = row.cells[0].firstChild;
    		    chk.checked = !chk.checked;
    		    if (chk.checked)
    		    {
    		        row.className="SelectedRow";
    		    }
    		    else
    		    {
    		       row.className="";
    		    }
    		}
    	}
    	function checkUncheckAll(theElement) {
     var theForm = theElement.form, z = 0;
	 // && theForm[z].name != 'checkall'
	 for(z=0; z<theForm.length;z++){
      if(theForm[z].type == 'checkbox'){
	  theForm[z].checked = theElement.checked;
	  }
     }
    }
    function checkCheckBox() {
     var theForm = document.forms[0], z = 0;
	 // && theForm[z].name != 'checkall'
	 for(z=0; z<theForm.length;z++){
      if(theForm[z].type == 'checkbox' && theForm[z].checked){
	    return true;
	  }
     }
     return false;
    }
    	function checkRowOfObject(obj)
    	{
    	    if (obj.checked)
		    {
		        obj.parentNode.parentNode.className="SelectedRow";
		    }
		    else
		    {
		       obj.parentNode.parentNode.className="";
		    }
    	}
    	function SelectAllRows()
    	{
    	   var chkAll = window.event.srcElement; 
    	   var tbl = chkAll.parentNode.parentNode.parentNode.parentNode;
    	   
    	   if (chkAll)
    	   {
    	        for(var i=1;i<tbl.rows.length;i++)//-1
    	        {
    	            var chk = tbl.rows[i].cells[0].firstChild;
    	            chk.checked=chkAll.checked;
    	            checkRowOfObject(chk);
    	        }
    	   }
    	}