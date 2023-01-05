var octopusInput = [
    // example data
    // '5483143223',
    // '2745854711',
    // '5264556173',
    // '6141336146',
    // '6357385478',
    // '4167524645',
    // '2176841721',
    // '6882881134',
    // '4846848554',
    // '5283751526',
    // puzzle data 
    '1224346384',
    '5621128587',
    '6388426546',
    '1556247756',
    '1451811573',
    '1832388122',
    '2748545647',
    '2582877432',
    '3185643871',
    '2224876627',
]; 
var logDays = [
    // example data
    //  1 -> +0           2 -> +35   (35)     3 -> +45  (80)      4 -> +16  (96)       5 ->  +8 (104)
    //  6 -> +1 (105)     7 -> +7   (112)     8 -> +24 (136)      9 -> +39 (175)      10 -> +29 (204)
    1,2,3,4,5,6,7,8,9,10,20,30,40,50,60,70,80,90,100
    // puzzle data
];
var intervalDuration = 500; // interval between animation renderings in milliseconds
var shouldDraw = true;
var extendedLog = false;

// set up everything with the correct width and height

var octopusWidth = 25;
var octopusHeight = 25;
var cavernWidth = octopusInput[0].length * octopusWidth; 
var cavernHeight = octopusInput.length * octopusHeight;

var cavernElement = document.getElementById("cavern");
cavernElement.width = cavernWidth;
cavernElement.height = cavernHeight;
var context = cavernElement.getContext("2d");

var logElement = document.getElementById("log");
var currentFlashesElement = document.getElementById("currentFlashes");

// octopus constants

var currentDay = 0;
var octopussies = new Array();
var allFlashes = 0;
var currentFlashes = 0;
var flashedAllAtOnce = false;

var octopussies = new Array(octopusInput[0].length);
for (var i = 0; i < octopussies.length; i++) {
    octopussies[i] = new Array(octopusInput.length);
}

// now start the logic

function octopus(status, x, y) {
    this.status = status;
    this.x = x;
    this.y = y;
    this.color = 360 * Math.random();
}

function renderAll() {
    if (shouldDraw) {
        drawCavern();
        drawOctopussies();
    }

    // age octopus
    currentFlashes = 0;
    for (var x = 0; x < octopussies.length; x++) {
        for (var y = 0; y < octopussies[x].length; y++) {
            octopussies[x][y].age();
        }
    }
    for (var x = 0; x < octopussies.length; x++) {
        for (var y = 0; y < octopussies[x].length; y++) {
            octopussies[x][y].reset();
        }
    }
    currentDay++;
    
    // log some specified days 
    if (logDays.includes(currentDay)) {
        logElement.innerHTML += "<p><b>Day " + currentDay + ":</b> " + allFlashes + "</p>"
        + (extendedLog ? ("<pre>" + stringifyOctopussies() + "</pre>") : "")
        ;
    }
    // puzzle result
    if (currentFlashes >= (octopussies.length * octopussies[0].length) && !flashedAllAtOnce) {
        logElement.innerHTML += "<p><b>Day " + currentDay + ":</b> " + allFlashes + "</p>"
            + (extendedLog ? ("<pre>" + stringifyOctopussies() + "</pre>") : "")
        ;
        flashedAllAtOnce = true;
    }
    // display the current flashes
    currentFlashesElement.innerHTML = "<b>Current Flashes:</b> " + currentFlashes;
}

function drawCavern() {
    context.fillStyle = "black";
    context.fillRect(0, 0, cavernWidth, cavernHeight);
}

function drawOctopussies() {
    for (var x = 0; x < octopussies.length; x++) {
        for (var y = 0; y < octopussies[x].length; y++) {
            drawOctopus(octopussies[x][y]);
        }
    }
}

function drawOctopus(octopus) {
    context.strokeStyle = 'hsl(' + octopus.color + ', 50%, ' + (octopus.status * 10) + '%)';
    context.font = "bold " + octopusWidth + "px monospace";
    context.textAlign = "center";
    context.textBaseline = "middle";
    context.strokeText('㊍', octopus.x * octopusWidth + octopusWidth / 2, octopus.y * octopusHeight + octopusHeight / 2 + octopusHeight / 20);
}

octopus.prototype.age = function() {
    // --- Day 11: Dumbo Octopus ---
    // You enter a large cavern full of rare bioluminescent dumbo octopuses! 
    // They seem to not like the Christmas lights on your submarine, so you 
    // turn them off for now.

    this.status++;
    
    if (this.status == 10) {
        // this octopus flashes
        allFlashes++;
        currentFlashes++;
        
        const neighbors = [ -1, 0, 1 ];
        for (const xNeighbor in neighbors) {
            for (const yNeighbor in neighbors) {
                if (neighbors[xNeighbor] != 0 || neighbors[yNeighbor] != 0) {
                    ageOctopusIfExists(this.x + neighbors[xNeighbor], this.y + neighbors[yNeighbor]);
                }
            }
        }
    } else if (this.status >= 10) {
        // we can ignore this, since this octopus doesn't do anything anymore
    } else {
        // if the status is < 9 nothing happens
    }
}

function ageOctopusIfExists(x, y) {
    if (x < 0 || y < 0) return;
    if (x >= octopussies.length || y >= octopussies[0].length) return;

    try {
    octopussies[x][y].age();
}catch (e) {
        alert(x + "x" + y +"   " + xNeighbor + "|" + yNeighbor)
    }
}


octopus.prototype.reset = function() {
    if (this.status >= 10) {
        this.status = 0;
    }
}

function stringifyOctopussies() {
    var result = "";
    for (var y = 0; y < octopussies[0].length; y++) {
        for (var x = 0; x < octopussies.length; x++) {
            result += octopussies[x][y].status;
        }
        result += "\n";
    }
    return result;
}

// Fill the cavern with animals

for (var y = 0; y < octopusInput.length; y++) {
    for (var x = 0; x < octopusInput[y].length; x++) {
        octopussies[x][y] = new octopus(parseInt(octopusInput[y].charAt(x)), x, y);
    }
}

if (extendedLog) {
    logElement.innerHTML += "<p><b>Day " + currentDay + ":</b> " + allFlashes + "</p>"
        + "<pre>" + stringifyOctopussies() + "</pre>"
    ;
}

// finally, start rendering
setInterval(function() {
    renderAll()
}, intervalDuration);