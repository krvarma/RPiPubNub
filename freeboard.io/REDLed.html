<html>
    <head>
    	<style>
    		input[type=checkbox] {
                visibility: hidden;
            }

			.container{
                
            }

            /* ROUNDED ONE */
            .redLed {
                width: 68px;
                height: 68px;
                background: #fcfff4;

                background: -webkit-linear-gradient(top, #fcfff4 0%, #dfe5d7 40%, #b3bead 100%);
                background: -moz-linear-gradient(top, #fcfff4 0%, #dfe5d7 40%, #b3bead 100%);
                background: -o-linear-gradient(top, #fcfff4 0%, #dfe5d7 40%, #b3bead 100%);
                background: -ms-linear-gradient(top, #fcfff4 0%, #dfe5d7 40%, #b3bead 100%);
                background: linear-gradient(top, #fcfff4 0%, #dfe5d7 40%, #b3bead 100%);
                filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#fcfff4', endColorstr='#b3bead',GradientType=0 );
                margin: 20px auto;

                -webkit-border-radius: 50px;
                -moz-border-radius: 50px;
                border-radius: 50px;

                -webkit-box-shadow: inset 0px 1px 1px white, 0px 1px 3px rgba(0,0,0,0.5);
                -moz-box-shadow: inset 0px 1px 1px white, 0px 1px 3px rgba(0,0,0,0.5);
                box-shadow: inset 0px 1px 1px white, 0px 1px 3px rgba(0,0,0,0.5);
                position: relative;
            }

            .redLed label {
                cursor: pointer;
                position: absolute;
                width: 60px;
                height: 60px;

                -webkit-border-radius: 50px;
                -moz-border-radius: 50px;
                border-radius: 50px;
                left: 4px;
                top: 4px;

                -webkit-box-shadow: inset 0px 1px 1px rgba(0,0,0,0.5), 0px 1px 0px rgba(255,255,255,1);
                -moz-box-shadow: inset 0px 1px 1px rgba(0,0,0,0.5), 0px 1px 0px rgba(255,255,255,1);
                box-shadow: inset 0px 1px 1px rgba(0,0,0,0.5), 0px 1px 0px rgba(255,255,255,1);

                background: -webkit-linear-gradient(top, #222 0%, #45484d 100%);
                background: -moz-linear-gradient(top, #222 0%, #45484d 100%);
                background: -o-linear-gradient(top, #222 0%, #45484d 100%);
                background: -ms-linear-gradient(top, #222 0%, #45484d 100%);
                background: linear-gradient(top, #222 0%, #45484d 100%);
                filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#222', endColorstr='#45484d',GradientType=0 );
            }

            .redLed label:after {
                -ms-filter: "progid:DXImageTransform.Microsoft.Alpha(Opacity=0)";
                filter: alpha(opacity=0);
                opacity: 0;
                content: '';
                position: absolute;
                width: 56px;
                height: 56px;
                background: #00bf00;

                background: -webkit-linear-gradient(top, #bf0000 0%, #940000 100%);
                background: -moz-linear-gradient(top, #bf0000 0%, #940000 100%);
                background: -o-linear-gradient(top, #bf0000 0%, #940000 100%);
                background: -ms-linear-gradient(top, #bf0000 0%, #940000 100%);
                background: linear-gradient(top, #bf0000 0%, #940000 100%);

                -webkit-border-radius: 50px;
                -moz-border-radius: 50px;
                border-radius: 50px;
                top: 2px;
                left: 2px;

                -webkit-box-shadow: inset 0px 1px 1px white, 0px 1px 3px rgba(0,0,0,0.5);
                -moz-box-shadow: inset 0px 1px 1px white, 0px 1px 3px rgba(0,0,0,0.5);
                box-shadow: inset 0px 1px 1px white, 0px 1px 3px rgba(0,0,0,0.5);
            }

            .redLed label:hover::after {
                -ms-filter: "progid:DXImageTransform.Microsoft.Alpha(Opacity=30)";
                filter: alpha(opacity=30);
                opacity: 0.3;
            }

            .redLed input[type=checkbox]:checked + label:after {
                -ms-filter: "progid:DXImageTransform.Microsoft.Alpha(Opacity=100)";
                filter: alpha(opacity=100);
                opacity: 1;
            }
    	</style>
    	<script src="https://cdn.pubnub.com/pubnub-3.7.10.js"/>
    	<script>
			var pubnub = PUBNUB({                          
				publish_key   : 'pub-c-60886db0-45cd-4921-9864-a4740e5493d8',
				subscribe_key : 'sub-c-075dba22-0cfe-11e4-8880-02ee2ddab7fe',
                ssl: true
			});
    
            function myFunction(pin, ctrl) {
                var isChecked = document.getElementById(ctrl).checked;
                var pinValue = isChecked ? 1 : 0;
                
                var msg = {
					"pin": pin,
                	"value": pinValue
				};
                
                console.log("Message: " + msg["pin"] + ", " + msg["value"]);
			
				pubnub.publish(
                    { 
                        channel : "rpipb-control",
                        message : msg,
                        callback: function(m){ console.log(m) }
                    }
				);
            }
		</script>
	</head>

	<body>
            <div align="center" vertical-align="middle" style="transform: translateY(50%)">
     			<div class="redLed">
                	<input type="checkbox" value="None" id="redled" name="check" onclick="myFunction(5, 'redled')">
                    <label for="redled"></label>
				</div>
			</div>
	</body>
</html>