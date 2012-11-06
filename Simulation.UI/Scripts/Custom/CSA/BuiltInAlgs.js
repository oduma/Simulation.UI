function aggregateAlgorythmPool(algorythmPool) {
    addTop123Algorythm(algorythmPool);
    addTopF1Algorythm(algorythmPool);
}

function addTop123Algorythm(algorythmPool) {
    var top123Algorythm = new ScoreAlgorythmType();
    top123Algorythm = { name: "Top 3 (3,2,1)", algorythm: function (scoreData) {
        if (scoreData.rank == 1)
            scoreData.score = 3;
        else if (scoreData.rank == 2)
            scoreData.score = 2;
        else if (scoreData.rank == 3)
            scoreData.score = 1;
        else if (scoreData.rank>3)
            scoreData.score = 0;
        }
    };
    algorythmPool.push(top123Algorythm);
}

function addTopF1Algorythm(algorythmPool) {
    var topF1Algorythm = new ScoreAlgorythmType();
    topF1Algorythm = { name: "F1", algorythm: function (scoreData) {
        if (scoreData.rank == 1) {
            scoreData.score = 10;
        }
        else if (scoreData.rank == 2) {
            scoreData.score = 8;
        }
        else if (scoreData.rank == 3) {
            scoreData.score = 6;
        }
        else if (scoreData.rank == 4) {
            scoreData.score = 5;
        }
        else if (scoreData.rank == 5) {
            scoreData.score = 4;
        }
        else if (scoreData.rank == 6) {
            scoreData.score = 3;
        }
        else if (scoreData.rank == 7) {
            scoreData.score = 2;
        }
        else if (scoreData.rank == 8) {
            scoreData.score = 1;
        }
        else if (scoreData.rank > 8) {
            scoreData.score = 0;
        }
        return;
    }
    };
    algorythmPool.push(topF1Algorythm);
}