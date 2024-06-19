import React, { useState, useEffect } from 'react';
import axios from 'axios';

const Scoreboard = () => {
    const [scores, setScores] = useState([]);

    useEffect(() => {
        axios.get('http://localhost:5000/scores/rank')
            .then(response => setScores(response.data))
            .catch(error => console.error(error));
    }, []);

    return (
        <div>
            <h2>Scoreboard</h2>
            <ul>
                {scores.map(score => (
                    <li key={score.PlayerId}>
                        {score.PlayerName}: {score.TotalPoints} points
                    </li>
                ))}
            </ul>
        </div>
    );
};

export default Scoreboard;
