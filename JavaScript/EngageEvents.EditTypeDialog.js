//This code is used to provide a reference to the radwindow "wrapper"
function GetRadWindow() {
    var oWindow = null;
    if (window.radWindow) oWindow = window.radWindow;
    else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
    return oWindow;
}

function ConfigureDialog() {
    //Get a reference to the radWindow wrapper
    var oWindow = GetRadWindow();

    //Obtain the argument 
    var oArg = oWindow.Argument;

    //Use the argument
    var oArea = document.getElementById("DialogInfoArea");
    oArea.value = oArg.TextValue;
    oArea.style.color = oArg.Color;
    oArea.style.backgroundColor = oArg.BackColor;
}

function OK_Clicked() {
    var oWindow = GetRadWindow();

    //Get current content of text area
    var oNewText = document.getElementById("DialogInfoArea").value;
    alert("Text to be returned to main page: " + oNewText);

    //Variant1: Passing the argument to the close method will result in the same behavior
    oWindow.close(oNewText);

    //Variant2: Possible to set the argument property of RadWindow here, and read it in the OnClientClose event handler!
    //oWindow.argument = oNewText;			
}

function Cancel_Clicked() {
    var oWindow = GetRadWindow();
    oWindow.close();
}
//show the window
function showDialog() {
    //Force reload in order to guarantee that the onload event handler of the dialog which configures it executes on every show.
    var oWnd = window.radopen(null, "DialogWindow");
    oWnd.setUrl(oWnd.get_navigateUrl());
}

//Called when a window is being shown. Good for setting an argument to the window 
function OnClientshow(radWindow) {
    //Get current content of textarea
    var oText = document.getElementById("InfoArea").value;

    //Create a new Object to be used as an argument to the radWindow
    var arg = new Object();
    //Using an Object as a argument is convenient as it allows setting many properties.
    arg.TextValue = oText;
    arg.Color = "red";
    arg.BackColor = "yellow";

    //Set the argument object to the radWindow		
    radWindow.Argument = arg;
}

function CallBackFunction(radWindow, returnValue) {
    var oArea = document.getElementById("InfoArea");
    if (returnValue) oArea.value = returnValue;
    else alert("No text was returned");
}

// Called when a window is being closed.
function OnClientclose(radWindow) {
    //Another option for passing a callback value
    //Set the radWindow.argument property in the dialog
    //And read it here --> var oValue = radWindow.argument;										
    //Do cleanup if necessary
}	
