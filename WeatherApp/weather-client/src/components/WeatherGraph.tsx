import React, { useEffect, useState } from 'react';
import { LineChart, Line, XAxis, YAxis, CartesianGrid, Tooltip, Legend, ResponsiveContainer } from 'recharts';

const WeatherGraph: React.FC = () => {
    const [weatherData, setWeatherData] = useState([]);

    useEffect(() => {
        fetch("http://localhost:5000/api/weather/min-max")
            .then(response => response.json())
            .then(data => setWeatherData(data))
            .catch(error => console.error("Error fetching weather data:", error));
    }, []);

    return (
        <ResponsiveContainer width="100%" height={400}> 
            
            <LineChart data={weatherData}>

                <CartesianGrid strokeDasharray="3 3" />
                <XAxis dataKey="City" />
                <YAxis />
                <Tooltip />
                <Legend />
                <Line type="monotone" dataKey="MinTemperature" stroke="#8884d8" />
                <Line type="monotone" dataKey="MaxTemperature" stroke="#82ca9d" />
            </LineChart>
        </ResponsiveContainer>
    );
};

export default WeatherGraph;
