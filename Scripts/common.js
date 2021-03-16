/*
Date: June 2013: 
GUI Version Added: v2.03.1
*/

//GroupNames can only contain alphanumeric characters or one of the following 5 characters:   ()*-_
//Normal inputs display a message to the user that they must remove characters
//Hidden fields cannot do this so me must remove the characters
function replaceSpecialChars(str) {
    str = str.replace(/ /g, "_");
    str = str.replace("&", "and");
   return str.replace(/[^A-Za-z-_0-9\w*()]/g, '-');
}

function unescapeInput(input) {
	return unescape(input);
}

function escapeInput(input) {
	if (input === undefined || input === "" || input === null) {
		return "";
	}
	return unescape(escape(input));
}

$(document).ready(function() {

	//Remove options from Link Type
	if(CKEDITOR != null) {
		CKEDITOR.on('dialogDefinition', function (ev) {
			// Take the dialog name and its definition from the event
			// data.
			var dialogName = ev.data.name;
			var dialogDefinition = ev.data.definition;

			// Check if the definition is from the dialog we're
			// interested on (the "Link" dialog).
			if (dialogName == 'link') {
				// Get a reference to the "Link Info" tab.
				var infoTab = dialogDefinition.getContents('info');

				// Get a reference to the link type
				var linkOptions = infoTab.get('linkType');

				// set the array to your preference
				linkOptions['items'] = [['URL', 'url'], ['E-mail', 'email']];
			}
		});
	}

	//Convert existing text into hyperlinks
	$('.linkify').linkify({
		target: "_blank"
	});

	//Set new links to use new window
	$(".linkify a").attr("target", "_blank");

	//Update the Textarea before submitting
	$('.ck-save').click(function () {
		for (instance in CKEDITOR.instances) {
			//console.log(instance);
			CKEDITOR.instances[instance].updateElement();
		}			
	});

});