var fishStatuses = [
    // example data
    3,4,3,1,2
    // puzzle data 
    // 3,5,2,5,4,3,2,2,3,5,2,3,2,2,2,2,3,5,3,5,5,2,2,3,4,2,3,5,5,3,3,5,2,4,5,4,3,5,3,2,5,4,1,1,1,5,1,4,1,4,3,5,2,3,2,2,2,5,2,1,2,2,2,2,3,4,5,2,5,4,1,3,1,5,5,5,3,5,3,1,5,4,2,5,3,3,5,5,5,3,2,2,1,1,3,2,1,2,2,4,3,4,1,3,4,1,2,2,4,1,3,1,4,3,3,1,2,3,1,3,4,1,1,2,5,1,2,1,2,4,1,3,2,1,1,2,4,3,5,1,3,2,1,3,2,3,4,5,5,4,1,3,4,1,2,3,5,2,3,5,2,1,1,5,5,4,4,4,5,3,3,2,5,4,4,1,5,1,5,5,5,2,2,1,2,4,5,1,2,1,4,5,4,2,4,3,2,5,2,2,1,4,3,5,4,2,1,1,5,1,4,5,1,2,5,5,1,4,1,1,4,5,2,5,3,1,4,5,2,1,3,1,3,3,5,5,1,4,1,3,2,2,3,5,4,3,2,5,1,1,1,2,2,5,3,4,2,1,3,2,5,3,2,2,3,5,2,1,4,5,4,4,5,5,3,3,5,4,5,5,4,3,5,3,5,3,1,3,2,2,1,4,4,5,2,2,4,2,1,4
]; 
var logDays = [
    // example data
    1,5,10,15,18,80
    // puzzle data
    // 1,5,10,20,30,40,60,80,100,125,150,175,200,225,250,256
];
var intervalDuration = 500; // interval between animation renderings in milliseconds
var shouldDrawFish = true;

// set up the canvas with the correct width and height

var tankWidth = 800; 
var tankHeight = 500;
var groundHeight = 25; 

var aquariumElement = document.getElementById("aquarium");
aquariumElement.width = tankWidth;
aquariumElement.height = tankHeight + groundHeight;
var context = aquariumElement.getContext("2d");

var logElement = document.getElementById("log");

// fish constants

var currentDay = 0;
var fishes = new Array();
var minFishWidth = 5;
var maxFishWidth = 20;
var minFishHeight = 5;
var maxFishHeight = 10;

function getRandomArbitrary(min, max) {
    return (Math.random() * (max - min)) + min;
}

function fish(status) {
    this.status = status;
    this.width = getRandomArbitrary(minFishWidth, maxFishWidth);
    this.height = getRandomArbitrary(minFishHeight, maxFishHeight);
    this.x = getRandomArbitrary(0, tankWidth - this.width);
    this.y = getRandomArbitrary(0, tankHeight - this.height);
    this.dx = getRandomArbitrary(-this.width, this.width);
    this.dy = getRandomArbitrary(-this.height, this.height);
    this.fillStyle = 'hsl(' + 360 * Math.random() + ', 50%, 50%)';
    this.strokeStyle = "white";
}

fish.prototype.age = function() {
    // handle the status (this is what the puzzle is actually about)
    if (this.status == 0) {
        this.status = 6;
        var babyFish = new fish(9);
        babyFish.x = this.x;
        babyFish.y = this.y;
        babyFish.fillStyle = this.fillStyle;
        fishes[fishes.length] = babyFish;
    } else {
        this.status--;
    }
}

fish.prototype.move = function() {
    // dx and dy give a tendency to go in one direction
    this.x = this.x + this.dx + getRandomArbitrary(-minFishWidth, minFishWidth);
    this.y = this.y + this.dy + getRandomArbitrary(-minFishHeight, minFishHeight);

    if (this.x < 0) {
        this.x = 0;
        this.dx = getRandomArbitrary(-this.width, this.width);
    }
    if (this.x > tankWidth - this.width) {
        this.x = tankWidth - this.width;
        this.dx = getRandomArbitrary(-this.width, this.width);
    }
    if (this.y < 0) {
        this.y = 0;
        this.dy = getRandomArbitrary(-this.height, this.height);
    }
    if (this.y > tankHeight - this.height) {
        this.y = tankHeight - this.height;
        this.dy = getRandomArbitrary(-this.height, this.height);
    }
}

function drawFish(fish) {
    // draw a boxy fish
    context.fillStyle = fish.fillStyle;
    context.fillRect(fish.x, fish.y, fish.width, fish.height);
    context.strokeStyle = fish.strokeStyle;
    context.strokeRect(fish.x, fish.y, fish.width, fish.height);

    context.textAlign = "center";
    context.textBaseline = "middle";
    context.strokeText(fish.status, fish.x + fish.width / 2, fish.y + fish.height / 2);
}

function renderAll() {
    if (shouldDrawFish) {
        drawTank();
        drawFishes();
    
        // move fish for next rendering
        for (i = 0; i < fishes.length; i++) {
            fishes[i].move();
        }
    }

    // don't age fish after we stop logging
    if (currentDay > Math.max(...logDays)) {
        return;
    }
    
    // age fish and create new ones
    for (i = 0; i < fishes.length; i++) {
        fishes[i].age();
    }
    currentDay++;
    
    // log some specified days 
    if (logDays.includes(currentDay)) {
        logElement.innerHTML += "<p><b>Day " + currentDay + ":</b> " + fishes.length + "</p>";
    }
}

function drawTank() {
    // water
    context.fillStyle = "dodgerblue";
    context.fillRect(0, 0, tankWidth, tankHeight);

    // grass
    context.fillStyle = "gray";
    context.fillRect(0, tankHeight, tankWidth, tankHeight + groundHeight);
}

function drawFishes() {
    for (i = 0; i < fishes.length; i++) {
        drawFish(fishes[i]);
    }
}

//
// Create the fish and render the fishtank
//
for (var i = 0; i < fishStatuses.length; i++) {
    fishes[i] = new fish(fishStatuses[i]);
}

setInterval(function() {
    renderAll()
}, intervalDuration);