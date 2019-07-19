
    //$("#txtdate1,#txtdate2,#instdate1,#instdate2,#instdate3,#instdate4,#delinq1,#delinq2,#delinq3,#delinq4,#discdate1,#discdate2,#discdate3,#discdate4,#nextbilldate1,#txtbillstartdate,#txtbillenddate,#txtpayoffgood,#txtinitialinstall,#txtdatetaxsale,#txtlastdayred").datepicker({
    //    changeMonth: true,
    //    changeYear: true,
    //    format: "mm/dd/yyyy",
    //    autoclose: true

   
        
    //});
   


$(function () {
    $("#txtdate1,#txtdate2").datepicker({
        changeMonth: true,
        changeYear: true,
        format: "mm/dd/yyyy",
        language: "tr",
        autoHide: true,
        orientation:"bottom"
    }).on('change', function () {
        //$('.datepicker').datepicker("hide");
        $('.datepicker').hide();
    });
})



   
