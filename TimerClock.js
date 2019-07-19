//aler1();
//show_Timerclock();

function aler1()
{
alert('yes');
}
	// Set the clock's font face:
	var myfont_face = "Georgia";

	// Set the clock's font size (in point):
	var myfont_size = "13";

	// Set the clock's font color:
	var myfont_color = "#0072e9";
	
	// Set the clock's background color:
	var myback_color = "#FFFFFF";

	// Set the text to display before the clock:
	var mypre_text = "00:00";

	// Set the width of the clock (in pixels):
	var mywidth = 300;
    
    var previoustime=new Date();
	// Display the time in 24 or 12 hour time?
	// 0 = 24, 1 = 12
	var my12_hour = 1;

	// How often do you want the clock updated?
	// 0 = Never, 1 = Every Second, 2 = Every Minute
	// If you pick 0 or 2, the seconds will not be displayed
	var myupdate = 1;

	// Display the date?
	// 0 = No, 1 = Yes
	var DisplayDate = 0;

/////////////// END CONFIGURATION /////////////////////////
///////////////////////////////////////////////////////////

// Browser detect code
        var ie4=document.all
        var ns4=document.layers
        var ns6=document.getElementById&&!document.all

// Global varibale definitions:

	var dn = "";
	var mn = "th";
	var old = "";


// For Version 4+ browsers, write the appropriate HTML to the
// page for the clock, otherwise, attempt to write a static
// date to the page.
	if (ie4||ns6) { document.write('<span id="TimerClockIE" class="ProdTextBox1"></span>'); } //style="width:'+mywidth+'px; background-color:'+myback_color+'"
	else if (document.layers) { document.write('<ilayer id="TimerClockPosNS" visibility="hide"><layer class="ProdTextBox"  id="TimerClockNS"></layer></ilayer>'); }//bgColor="'+myback_color+'" width="'+mywidth+'"
	else { old = "true"; show_Timerclock(); }

// The main part of the script:
	function show_Timerclock(previoustime) {
		if (old == "die") { return; }
	 
	//show clock in NS 4
		if (ns4)
        document.TimerClockPosNS.visibility="show"
	// Get all our date variables:
	    var date1=previoustime;
	    
	  // var date1=starttime;
		var date2 = new Date();
		var diff= new Date();
		
		var currentTime=timediff(date2,date1);
            		

	// This is the actual HTML of the clock. If you're going to play around
	// with this, be careful to keep all your quotations in tact.
		myclock = '';
		//myclock += '<font style="color:'+myfont_color+';font-family:'+myfont_face+'; font-size:'+myfont_size+'pt;">';
		//myclock += mypre_text;
		myclock += currentTime;
		//myclock += '</font>';

		if (old == "true") {
			document.write(myclock);
			old = "die";
			return;
		}

	// Write the clock to the layer:
		if (ns4) {
			clockpos = document.TimerClockPosNS;
			liveclock = clockpos.document.TimerClockNS;
			liveclock.document.write(myclock);
			liveclock.document.close();
		} else if (ie4) {
			TimerClockIE.innerHTML = myclock;
		} else if (ns6){
			document.getElementById("TimerClockIE").innerHTML = myclock;
                }            

	var timeOut = setTimeout("show_Timerclock(previoustime)",1000);
}


 function setTwoDigits(digit)
    {
        if(digit<10)
        {
            digit = "0" + digit;
        }
        return digit;
    } 
    
    function timediff(time1,time2)//time1 is the running time, time2 is the fixed time
    {
    //alert(time1.toGMTString());
    
    	//document.getElementById('clockTimer1').innerHTML="inside timediff";
    	h1=parseInt(time1.getHours());
    //	h2=parseInt(time2.getHours());
    	m1= parseInt(time1.getMinutes());
    //	m2= parseInt(time2.getMinutes());
    	s1=parseInt(time1.getSeconds());
    //	s2=parseInt(time2.getSeconds());
//    	
        
    	var times=time2.split(":");
    	
        h2=parseInt(times[0]);
        m2=parseInt(times[1]);
        s2=parseInt(times[2]);
        //alert(s1);
//        if(h1>12)
//        {
//        h1=h1-12;
//        }
//        
//        if(h2>12)
//        {
//        h2=h2-12;
//        }
    	
    	s3=seeGreat(s1,s2);
    	if (s3==-1)
    	{
    		s1=s1+60;
    		if(m1==0)
    		{
    			h1=h1-1;
    			m1=60;
    		}
    	else
    		{
    			m1=m1-1;
    		}
    		s3=seeGreat(s1,s2);
    	}
    	m3=seeGreat(m1,m2);
    	if (m3==-1)
    	{
    		m1=m1+60;
    		if(h1==0)
    		{
    			h1=0;
    		}
    	else
    		{
    			h1=h1-1;
    		}
    		m3=seeGreat(m1,m2);
    	}
    	h3=seeGreat(h1,h2);
    	if(h1==-1)
    	{
    		h3=0;
    	}
    	if(h3<0)
    	{
    	h3=0;
    	}
    	if(h1<h2)
    	{
    	h3=0;
    	m3=0;
    	}
    	se=setTwoDigits(s3);
    	mi=setTwoDigits(m3);
    	ho=setTwoDigits(h3);
    	//return (ho+ ":" + mi + ":" + se);
    	return (mi + ":" + se);
    	
}
function seeGreat(no1,no2)
{
	//document.getElementById('clockTimer1').innerHTML="no1:" +no1 +" no2:"+no2 ;
	if(no1==no2)
	{
		return 0;
	}
	if(no1>no2)
	{
		return (no1-no2);
	}
	if(no1<no2)
	{
		return -1;
	}

}

function getTimes(ddate)
{
var times=ddate.split(":");
ho=parseInt(times[0]);
mi=parseInt(times[1]);
se=parseInt(times[2]);
}
function setTimes(sdate)
{
h=parseInt(sdate.getHours());
m= parseInt(sdate.getMinutes());
s=parseInt(sdate.getSeconds());
return (h+ ":" + m + ":" + s);

}

function setTimeValue(id)
{
y=document.getElementById(id);
alert(y.value);
if(y.value=="")
{
y.value=setTimes(new Date());
}
show_Timerclock(y.value);
}


//Settting cookie

function createCookie(name,value,days) {
	if (days) {
		var date = new Date();
		date.setTime(date.getTime()+(days*24*60*60*1000));
		var expires = "; expires="+date.toGMTString();
	}
	else var expires = "";
	document.cookie = name+"="+value+expires+"; path=/";
}

function readCookie(name) {
	var nameEQ = name + "=";
	var ca = document.cookie.split(';');
	for(var i=0;i < ca.length;i++) {
		var c = ca[i];
		while (c.charAt(0)==' ') c = c.substring(1,c.length);
		if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length,c.length);
	}
	return null;
}

function eraseCookie(name) {
	createCookie(name,"",-1);
}
// JScript File

function show_TickerTime() {
		if (old == "die") { return; }
	 
	//show clock in NS 4
		if (ns4)
        document.TimerClockPosNS.visibility="show"
	// Get all our date variables:
	    y=document.getElementById('theInput');
   //     alert(y.value);
        if(y.value=="")
        {
        y.value=setTimes(new Date());
        alert("Value of y is empty");
        }
        
	    var date1=y.value;
	    
	  
		var date2 = new Date();
		//alert(date2);
		
		var currentTime=timediff(date2,date1);
            		

	// This is the actual HTML of the clock. If you're going to play around
	// with this, be careful to keep all your quotations in tact.
		myclock = '';
		myclock += currentTime;
		if (old == "true") {
			document.write(myclock);
			old = "die";
			return;
		}

	// Write the clock to the layer:
		if (ns4) {
			clockpos = document.TimerClockPosNS;
			liveclock = clockpos.document.TimerClockNS;
			liveclock.document.write(myclock);
			liveclock.document.close();
		} else if (ie4) {
			TimerClockIE.innerHTML = myclock;
		} else if (ns6){
			document.getElementById("TimerClockIE").innerHTML = myclock;
                }            

	var timeOut = setTimeout("show_TickerTime()",1000);
}