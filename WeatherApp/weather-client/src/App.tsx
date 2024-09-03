import React from 'react';
import WeatherGraph from './components/WeatherGraph';

const App: React.FC = () => {
    return (
        <div className="app-container">
            <header className="app-header">
                <h1>Weather Data</h1>
            </header>
            <main className="app-main">
                <WeatherGraph />
            </main>
            <footer className="app-footer">
                <p>Weather data powered by OpenWeatherMap</p>
            </footer>
        </div>
    );
};

export default App;
