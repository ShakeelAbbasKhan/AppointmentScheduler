var routeURL = location.protocol + "//" + location.host;
$(document).ready(function () {
    $("#appointmentDate").kendoDateTimePicker({
        value: new Date(),
          //var formattedDate = moment(obj.startStr).format("M/D/YYYY h:mm A");
    //$("#appointmentDate").val(formattedDate);
        dateInput: false
    });

    InitializeCalendar();
});
var calendar;
function InitializeCalendar() {
    try {


        var calendarEl = document.getElementById('calendar');
        if (calendarEl != null) {
           calendar = new FullCalendar.Calendar(calendarEl, {
                initialView: 'dayGridMonth',
                headerToolbar: {
                    left: 'prev,next,today',
                    center: 'title',
                    right: 'dayGridMonth,timeGridWeek,timeGridDay'
                },
                selectable: true,
                editable: false,
                select: function (event) {
                    onShowModal(event, null);
                },
                
                eventDisplay: 'block',
                events: function (fetch, successCallback, failureCallback) {
                    //debugger;
                    $.ajax({
                        url: routeURL + '/Appointment/GetCalendarData?doctorId=' + $("#doctorId").val(),
                        type: 'GET',
                        dataType: 'JSON',
                        success: function (response) {
                            //debugger;
                            var events = [];
                            if (response.status === 1) {
                                
                                $.each(response.dataemum, function (i, data) {
                                    events.push({
                                        title: data.title,
                                        description: data.description,
                                        start: data.startDate,
                                        end: data.endDate,
                                        backgroundColor: data.isDoctorApproved ? "#28a745" : "#dc3545",
                                        borderColor: "#162466",
                                        textColor: "white",
                                        id: data.id
                                    });
                                })
                            }
                            successCallback(events);
                           // debugger;
                        },
                        error: function (xhr) {
                            $.notify("Error", "error");
                        }
                    });
                },
                eventClick: function (info) {
                    getEventDetailsByEventId(info.event);
                }
                });
            calendar.render();
        }

    }
    catch (e) {
        alert(e);
    }

}

function onShowModal(obj, isEventDetail) {
    if (isEventDetail != null) {
        debugger;

        $("#title").val(obj.title);
        $("#description").val(obj.description);
        $("#appointmentDate").val(obj.startDate);
        $("#duration").val(obj.duration);
        $("#doctorId").val(obj.doctorId);
        $("#patientId").val(obj.patientId);
        $("#id").val(obj.id);
        $("#lblPatientName").html(obj.patientName);
        $("#lblDoctorName").html(obj.doctorName);
        if (obj.isDoctorApproved) {
            $("#lblStatus").html('Approved');

            $("#btnConfirm").addClass("d-none");  // if confirm the appointment then not show the buttons
            $("#btnSubmit").addClass("d-none");  // if submit the appointment then not show the buttons
        }
        else {
            $("#lblStatus").html('Pending');
            $("#btnSubmit").addClass("d-none")
        }
        $("#btnDelete").removeClass("d-none")
        
        

    }
    else {
    var formattedDate = moment(obj.startStr).format("M/D/YYYY h:mm A");
        $("#appointmentDate").val(formattedDate);
        $("#btnDelete").addClass("d-none")
        $("#btnSubmit").removeClass("d-none")
    }


    $("#appointmentInput").modal("show");
}

function onCloseModal() {
    $("#appointmentForm")[0].reset();
    $("#id").val(0),
    $("#title").val('');
    $("#description").val('');
    $("#appointmentDate").val('');
    //$("#duration").val('');
    //$("#patientId").val('');      // from this first value of their dropdown show at first we come to model
    $("#appointmentInput").modal("hide");
}

function onSubmitForm() {
    if (checkValidation()) {
        var requestData = {
            /*Id: parseInt($("#id").val()),*/
            Title: $("#title").val(),
            Description: $("#description").val(),
            StartDate: $("#appointmentDate").val(),
            Duration: $("#duration").val(),
            DoctorId: $("#doctorId").val(),
            PatientId: $("#patientId").val(),
        };
        //debugger;
        $.ajax({
            url: routeURL + '/Appointment/SaveCalendarData',
            type: 'POST',
            data: JSON.stringify(requestData),
            contentType: 'application/json',
            success: function (response) {
                if (response.status === 1 || response.status === 2) {
                    calendar.refetchEvents();
                //    debugger;
                    $.notify(response.message, "success");
                    onCloseModal();
                }
                else {
                    $.notify(response.message, "error");
                }
            },
            error: function (xhr) {
                $.notify("Error", "error");
            }
        });
    }
}

function checkValidation() {
    var isValid = true;
    if ($("#title").val() === undefined || $("#title").val() === "") {
        isValid = false;
        $("#title").addClass('error');
    }
    else {
        $("#title").removeClass('error');
    }

    if ($("#appointmentDate").val() === undefined || $("#appointmentDate").val() === "") {
        isValid = false;
        $("#appointmentDate").addClass('error');
    }
    else {
        $("#appointmentDate").removeClass('error');
    }

    return isValid;

}

function getEventDetailsByEventId(info) {
  //  debugger;
    $.ajax({
        url: routeURL + '/Appointment/GetCalendarDataById/' + info.id,
        type: 'GET',
        dataType: 'JSON',
        success: function (response) {
          //  debugger;
            if (response.status === 1 && response.dataemum !== undefined) {
                
                onShowModal(response.dataemum, true);
            }
           // successCallback(events);
        },
        error: function (xhr) {
            $.notify("Error", "error");
        }
    });
}

function OnSelectDoctorChange() {
    calendar.refetchEvents();
}

function confirmDeleteAppointment() {
    var confirmed = confirm("Are you sure you want to delete this appointment?");
    if (confirmed) {
        onDeleteAppointment();
    }
}

function onDeleteAppointment() {
    var id = parseInt($("#id").val());
    $.ajax({
        url: routeURL + '/Appointment/DeleteAppointment/' + id,
        type: 'GET',
        dataType: 'JSON',
        success: function (response) {
            //  debugger;
            if (response.status === 1) {
                $.notify(response.message, "success");
                calendar.refetchEvents();
                onCloseModal();
            }
            else {
                $.notify(response.message, "error");
            }
        },
        error: function (xhr) {
            $.notify("Error", "error");
        }
    });
}

function onConfrim() {
    var id = parseInt($("#id").val());
    $.ajax({
        url: routeURL + '/Appointment/ConfirmAppointment/' + id,
        type: 'GET',
        dataType: 'JSON',
        success: function (response) {
            //  debugger;
            if (response.status === 1) {
                $.notify(response.message, "success");
                calendar.refetchEvents();
                onCloseModal();
            }
            else {
                $.notify(response.message, "error");
            }
        },
        error: function (xhr) {
            $.notify("Error", "error");
        }
    });
}