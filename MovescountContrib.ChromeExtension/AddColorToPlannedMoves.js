$("ul.items.menu").each(function (i, e) {
  var regex = /(icon-\d+)/;
  var planned = $(e).find("i.planned-move");
  var moves = $(e).find("i:not(.planned-move)");
  
  var completedMoves = $.map(moves, function (el, i) {
    var match = regex.exec($(el).attr("class"));
    if (match && match.length > 1) return match[1];
    else  return "";
  });
  
  $.each(planned, function (i, el) {
    var match = regex.exec($(el).attr("class"));
    if (match && match.length > 1) {
      var plannedMoveType = match[1];
      if (completedMoves.indexOf(plannedMoveType) > -1) {
        $(el).attr("style", "background-color: rgba(168, 255, 152, 1.0) !important");
      } else {
        $(el).attr("style", "background-color: rgba(255, 175, 175, 1.0) !important");
      }
    }
  });
});