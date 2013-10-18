/// <reference path="../kinetic/kinetic-v4.7.2.js" />

window.board = window.board || (function () {
    var self = this;

    var layer;
    var squares;
    var symbol;
    var rectClickCallback;

    function squaresCollection() {
        var self = this;
        self.squares = [];

        self.addSquare = function (square, x, y) {
            self.squares[y * 3 + x] = square;
        };
        self.getSquare = function (x, y) {
            return self.squares[y * 3 + x];
        };
    }

    function getSquareMiddle(square) {
        var x = square.getX();
        var y = square.getY();
        var width = square.getWidth();
        var heigh = square.getHeight();

        return [x + (width / 2), y + (heigh / 2)];
    }

    function setRectValue(square, value) {
        if (square.value == null && value != null) {
            square.value = value;

            var x = square.getX();
            var y = square.getY();
            var width = square.getWidth();
            var height = square.getHeight();
            square.remove();

            if (value == 'O') {
                var radius;
                if (width > height) {
                    radius = height / 2;
                }
                else {
                    radius = width / 2;
                }

                var circle = new Kinetic.Circle({
                    radius: radius - 5,
                    stroke: 'black',
                    x: x + (width / 2),
                    y: y + (height / 2),
                    strokeWidth: 5
                });
                layer.add(circle);
            }
            else {
                var offset = 3;

                var upperLeft = [x + offset, y + offset];
                var upperRight = [x + width - offset, y + offset];
                var bottomLeft = [x + offset, y + height - offset];
                var bottomRight = [x + width - offset, y + height - offset];

                var line = new Kinetic.Line({
                    points: [upperLeft, bottomRight],
                    stroke: 'black',
                    strokeWidth: 5
                });
                layer.add(line);
                line = new Kinetic.Line({
                    points: [upperRight, bottomLeft],
                    stroke: 'black',
                    strokeWidth: 5
                });
                layer.add(line);
            }

            layer.draw();
        }
    }

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
        var rect = evt.targetNode;
        rectClickCallback(rect.xIndex, rect.yIndex, symbol);
    }

    function init(id, height, width, callback) {
        rectClickCallback = callback;

        var stage = new Kinetic.Stage({
            container: id,
            height: height,
            width: width
        });

        layer = new Kinetic.Layer();
        squares = new squaresCollection();

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
                    height: thirdHeight
                });

                rect.xIndex = xIndex;
                rect.yIndex = yIndex;
                rect.value = null;

                rect.on('mouseover', rectMouseOver);
                rect.on('mouseout', rectMouseOut);
                rect.on('click', rectClick);

                layer.add(rect);
                squares.addSquare(rect, xIndex, yIndex);
            });
        });

        // draw lines
        var line = new Kinetic.Line({
            points: [[thirdWidth, 0], [thirdWidth, height]],
            stroke: 'black'
        });
        layer.add(line);
        line = new Kinetic.Line({
            points: [[thirdWidth * 2, 0], [thirdWidth * 2, height]],
            stroke: 'black'
        });
        layer.add(line);
        line = new Kinetic.Line({
            points: [[0, thirdHeight], [width, thirdHeight]],
            stroke: 'black'
        });
        layer.add(line);
        line = new Kinetic.Line({
            points: [[0, thirdHeight * 2], [width, thirdHeight * 2]],
            stroke: 'black'
        });
        layer.add(line);

        stage.add(layer);
    }

    function setPlayerSymbol(playerSymbol) {
        symbol = playerSymbol;
    }

    function updateBoard(board) {
        $.each(board, function (xIndex, row) {
            $.each(row, function (yIndex, squareValue) {
                var square = squares.getSquare(xIndex, yIndex);
                setRectValue(square, squareValue);
            });
        });
    }

    function endGame(winningLine) {
        for (i = 0; i < 3; i++) {
            for (j = 0; j < 3; j++) {
                var square = squares.getSquare(i, j);
                square.remove();
            }
        }

        if (winningLine) {
            var points = [];
            for (i = 0; i < 3; i++) {
                var squareX = winningLine[i][0];
                var squareY = winningLine[i][1];

                points.push(getSquareMiddle(squares.getSquare(squareX, squareY)));
            }

            layer.add(new Kinetic.Line({
                points: points,
                stroke: 'Red',
                strokeWidth: 5
            }));
        }
    }

    return {
        init: init,
        updateBoard: updateBoard,
        setPlayerSymbol: setPlayerSymbol,
        endGame: endGame
    };
}());