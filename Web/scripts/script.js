$(function () {
    $("#bars li .bar").each(function (key, bar) {
        var percentage = $(this).data('percentage');

        $(this).animate({
            'height': percentage + '%'
        }, 1000);
    });

});

$(".toggleAction > .fa-bars").click(
  function () {
      $(".mobileNavContainer").css("transform", " translateX(0%)");
      $(".toggleAction > .fa-times").css("display", " block");
      $(".toggleAction > .fa-bars").css("display", " none");
      $(".toggleIcon ").css("background", " #0b82aa");
      $("body").removeClass("colosed").addClass("open");
      $(".toggleIcon").css("border-bottom", "2px solid rgb(11,130,170)");
      $(".navLeft").css("border-bottom", "2px solid white");
      $(".navRight").css("border-bottom", "2px solid white");
  });

$(".toggleAction > .fa-times , .mobileNavBg").click(
  function () {
      $(".mobileNavContainer").css("transform", " translateX(-100%)");
      $(".toggleAction > .fa-times").css("display", " none");
      $(".toggleAction > .fa-bars").css("display", " block");
      $(".toggleIcon ").css("background", " #0095c7");
      $("body").removeClass("open").addClass("colosed");
      $(".toggleIcon").css("border-bottom", "none");
      $(".navLeft").css("border-bottom", "none");
      $(".navRight").css("border-bottom", "none");

  });


$(function () {
    $(".popupExit").click(
      function (e) {
          $(".overlay").css("visibility", " hidden");
          $(".view-client-popup").css("visibility", " hidden");
          $(".add-client-popup").css("visibility", " hidden");
      });
});

var options = {
    valueNames: ['hash', 'name', 'status', 'number', 'date']
};

var userList = new List('users', options);
