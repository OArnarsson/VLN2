<<<<<<< Updated upstream
=======
﻿var clock;
>>>>>>> Stashed changes

$(document).ready(function () {
    
    $('.TimeLeft').each(function (i, selected) {
        //to see the timeFormat of the value:  
        TheClock(selected.id, $(selected).val());
    });
      
    function TheClock(id, val) {
        var currentDate = new Date();
        var futureDate = new Date(val);
        var difference = (futureDate.getTime() / 1000) - (currentDate.getTime() / 1000);

        if (difference > 0) {
            var clock = $("." + id).FlipClock(difference, {
                clockFace: 'DailyCounter',
                countdown: true,
                showSeconds: true
            });
        }
        else {
            $("#rem"+ id).hide();
        }
    }
});


	
