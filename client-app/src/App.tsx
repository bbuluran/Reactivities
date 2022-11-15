import React, { useEffect, useState } from 'react';
import logo from './logo.svg';
import './App.css';
import axios from 'axios';
import { Header, List } from 'semantic-ui-react';
// import { Header, List } from 'semantic-ui-react';

function App() {
  // declare access modifier 'const'
  // [activities, setActivities] -> activities = variable where state will be stored, 
  //                             -> setActivities = function we use to set state
  // useState([]) -> [] = declare initial state of 'activities' variable

  // REACT hooks -> useState allow us to store state in component
  // REACT hooks -> useEffect allow us to create side effects when a component initializes
  
  const [activities, setActivities] = useState([]);

  useEffect(() => {
    axios.get('http://localhost:5000/api/activities').then(response => {
      console.log(response);
      setActivities(response.data);
    });
  }, []);


  return (
    <div>
      <Header as='h2' icon='users' content='Reactivities' />
      <List>
          {activities.map((activity: any) => (
            <List.Item key={activity.id}>
              {activity.title}
            </List.Item>
          ))}
      </List>
    </div>
  );
}

export default App;
