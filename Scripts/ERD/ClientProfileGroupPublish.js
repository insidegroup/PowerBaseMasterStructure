

$(document).ready(function () {

	$("tr:odd").addClass("row_odd");
	$("tr:even").addClass("row_even");

	$('#form0').submit(function (event) {

		event.preventDefault();

		var isProfileReadytoPublish = false;

		//When the GDS is Sabre, the profile format could be Sabre STARS or Sabre Profiles.
		//Prompt the CWT user to identify the Sabre format.  

		var gdsCode = $('#ClientProfileGroup_GDSCode').val();
		var sabreFormat = $('#SabreFormat');
		var sabre_dialog = $("#sabre-outer");
		var sabre_message = $("#sabre-message");

		if (gdsCode == "1S") {

			//Sabre Screen
			sabre_dialog.dialog({
				resizable: true,
				modal: true,
				height: 250,
				width: 600,
				title: "Please Select Sabre Format",
				buttons: {
					"Sabre STARS": function () {
						sabre_message.html("");
						$(this).dialog("close");
						sabreFormat.val("SabreStar");
						CheckProfile();
					},
					"Sabre Profile": function () {
						sabre_message.html("");
						$(this).dialog("close");
						sabreFormat.val("SabreProfile");
						isComplete = VerifyProfile();
					}
				}
			});
			$("#sabre-message").html('<p>Select which Sabre Format</p>');

		} else {
			CheckProfile();
		}

	});

	var defaultMessage = '<p><img src="/images/common/grid-loading.gif" alt="Loading" id="loader"></p>';
	var statusMessage = defaultMessage;

	function CheckProfile() {

		var success = false;

		//Loading Screen
		var dialog = $("#dialog-outer");
		dialog.html('<div id="dialog-message" style="text-align:center;"></div>');
		
		//Message
		var message = $("#dialog-message");
		statusMessage = defaultMessage;
		statusMessage += '<p>Checking...</p>';
		message.html(statusMessage);

		dialog.dialog({
			resizable: true,
			modal: true,
			height: 200,
			width: 600,
			title: "Checking Profile"
		});

		//Check if profile can be published
		//Checks line count and mandatory fields present
		$.ajax({
			url: "/ClientProfileGroup.mvc/IsProfileReadytoPublish",
			type: "POST",
			dataType: "json",
			data: { clientProfileGroupId: $("#ClientProfileGroup_ClientProfileGroupId").val() },
			async: false,
			success: function (data) {
				if (data == "true") {
					statusMessage += '<p style="color: green;">Check successful</p>';
					message.html(statusMessage);
					VerifyProfile();
				} else {
					dialog.dialog({
						resizable: true,
						modal: true,
						height: 200,
						width: 600,
						title: "Client Profile Confirmation",
						buttons: {
							"OK": function () {
								message.html("");
								$(this).dialog("destroy");
							}
						}
					});
					statusMessage += '<p style="color: red;">' + data + '</p>';
					statusMessage += '<p style="color: red;">No changes have been made to the GDS Client Profile.</p>';
					message.html(statusMessage);
					$('#loader').remove();
				}
			}
		});
	}

	function VerifyProfile() {

		//Loading Screen
		var gdsCode = $('#ClientProfileGroup_GDSCode').val();		
		var dialog = $("#dialog-outer");
		dialog.html('<div id="dialog-message" style="text-align:center;"></div>');
		
		//Message
		var message = $("#dialog-message");
		statusMessage += '<p>Verifying...</p>';
		message.html(statusMessage);

		dialog.dialog({
			resizable: true,
			modal: true,
			height: 200,
			width: 600,
			title: "Client Profile Verify"
		});

		$.ajax({
			url: "/ClientProfileGroup.mvc/Verify",
			type: "POST",
			data: {
				clientProfileGroupId: $("#ClientProfileGroup_ClientProfileGroupId").val(),
				sabreStatus: $('#SabreFormat').val(),
				gdsCode: gdsCode
			},
			async: false,
			dataType: "json",
			success: function (data) {
				if (data == "true") {
					statusMessage += '<p style="color: green;">Verify successful</p>';
					message.html(statusMessage);
					PublishProfile();
				} else {
					dialog.dialog({
						resizable: true,
						modal: true,
						height: 250,
						width: 600,
						title: "Client Profile Verify",
						buttons: {
							"OK": function () {
								dialog.html("");
								$(this).dialog("destroy");
								if (data == "true") {
									location.href = window.location.href;
								}
							}
						}
					});
					statusMessage += '<p style="color: red;">' + data + '</p>';
					statusMessage += '<p style="color: red;">No changes have been made to the GDS Client Profile.</p>';
					message.html(statusMessage);
					$('#loader').remove();
				}
			}
		});
	}

	function PublishProfile() {

		//Loading Screen
		var gdsCode = $('#ClientProfileGroup_GDSCode').val();	
		var dialog = $("#dialog-outer");
		dialog.html('<div id="dialog-message" style="text-align:center;"></div>');
		
		//Message
		var message = $("#dialog-message");
		statusMessage += '<p>Publishing...</p>';
		message.html(statusMessage);
	
		dialog.dialog({
			resizable: true,
			modal: true,
			height: 200,
			width: 600,
			title: "Client Profile Publish"
		});

		$.ajax({
			url: "/ClientProfileGroup.mvc/Publish",
			type: "POST",
			dataType: "json",
			data: {
				clientProfileGroupId: $("#ClientProfileGroup_ClientProfileGroupId").val(),
				sabreStatus: $('#SabreFormat').val(),
				gdsCode: gdsCode
			},
			async: false,
			success: function (data) {

				//Show Message
				dialog.dialog({
					resizable: true,
					modal: true,
					height: 250,
					width: 600,
					title: "Client Profile Publish",
					buttons: {
						"OK": function () {
							message.html("");
							$(this).dialog("destroy");
							if (data == "true") {
								location.href = window.location.href;
							}
						}
					}
				});
				if (data == "true") {
					statusMessage += '<p style="color: green;">Publish successful</p>';
				} else {
					statusMessage += '<p style="color: red;">' + data + '</p>';
					statusMessage += '<p style="color: red;">The GDS Client Profile may now be blank due to the publish process not completing.</p>';
				}
				message.html(statusMessage);
				$('#loader').remove();
			}
		});
	}

});