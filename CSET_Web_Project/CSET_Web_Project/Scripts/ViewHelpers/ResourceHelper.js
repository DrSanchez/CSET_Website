$('#mainBodyContainer').ready()
{
	sizeResourcePage();
}

/* Sets relative sizing for page loading */
function sizeResourcePage()
{
	//view width
	var bodyWidth = $('#resourceContainer').width();

	//set document container width, child divs inherit
	$('#docsContainer').width(bodyWidth * 0.33);
	
}