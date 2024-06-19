import React, { useState, useEffect } from 'react';
import axios from 'axios';

const PlayerList = () => {
    const [players, setPlayers] = useState([]);

    useEffect(() => {
        axios.get('http://localhost:5000/players')
            .then(response => setPlayers(response.data))
            .catch(error => console.error(error));
    }, []);

    return (
        <div>
            <h2>Players</h2>
            <ul>
                {players.map(player => (
                    <li key={player.id}>{player.name}</li>
                ))}
            </ul>
        </div>
    );
};

export default PlayerList;
