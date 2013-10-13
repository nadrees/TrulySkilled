/// <reference path="../kinetic/kinetic-v4.7.2.js" />

window.board = window.board || (function () {
    var layer;

    function rectMouseOver(evt) {
        var rect = evt.targetNode;
        rect.setFill('#00D2FF');
        layer.draw();
    }

    function rectMouseOut(evt) {
        var rect = evt.targetNode;
        rect.setFill(null);
        layer.draw();
    }

    function rectClick(evt) {
        var serializedStage = stage.toJSON();
        debugger
    }

    function init(id, height, width) {
        var stage = new Kinetic.Stage({
            container: id,
            height: height,
            width: width
        });

        layer = new Kinetic.Layer();

        var thirdWidth = width / 3;
        var thirdHeight = height / 3;

        // draw rectangles
        var validXValues = [0, thirdWidth, thirdWidth * 2];
        var validYValues = [0, thirdHeight, thirdHeight * 2];
        $.each(validXValues, function (xIndex, x) {
            $.each(validYValues, function (yIndex, y) {
                var rect = new Kinetic.Rect({
                    x: x,
                    y: y,
                    width: thirdWidth,
                    height: thirdHeight,
                    stroke: 'black'
                });
                rect.on('mouseover', rectMouseOver);
                rect.on('mouseout', rectMouseOut);
                rect.on('click', rectClick);

                layer.add(rect);
            });
        });

        stage.add(layer);
    }

    return {
        init: init
    };
}());