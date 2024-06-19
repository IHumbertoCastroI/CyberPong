import React from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import PongGame from './PongGame';
import PlayerList from './PlayerList';
import Scoreboard from './Scoreboard';

const App = () => {
    return (
        <Router>
            <Routes>
                <Route path="/" element={<PongGame />} />
                <Route path="/players" element={<PlayerList />} />
                <Route path="/scoreboard" element={<Scoreboard />} />
            </Routes>
        </Router>
    );
};

export default App;
