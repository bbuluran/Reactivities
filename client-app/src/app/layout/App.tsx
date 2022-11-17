import React, { Fragment, useEffect } from 'react';
import { Container } from 'semantic-ui-react';
import NavBar from './NavBar';
import ActivityDashboard from '../../features/activities/dashboard/ActivityDashboard';
import LoadingComponent from './LoadingComponent';
import { useStore } from '../stores/store';
import { observer } from 'mobx-react-lite';

function App() {
  // declare access modifier 'const'
  // [activities, setActivities] -> activities = variable where state will be stored, 
  //                             -> setActivities = function we use to set state
  // useState([]) -> [] = declare initial state of 'activities' variable

  // REACT hooks -> useState allow us to store state in component
  // REACT hooks -> useEffect allow us to create side effects when a component initializes
  // ex.   const [activities, setActivities] = useState<Activity[]>([]);


  const {activityStore} = useStore();

  useEffect(() => {
    activityStore.loadActivities();
  }, [activityStore]);

  if (activityStore.loadingInitial) return <LoadingComponent content='Loading app' />

  return (
    <Fragment>
      <NavBar />
      <Container style={{marginTop: '7em'}}>
        <ActivityDashboard />
      </Container>
    </Fragment>
  );
}

// if we want to make our components observe changes in our store,
// we need to wrap the 'App' function to the observer.
// This observer higher order function returns the 'App' function,
// with additional powers to observe the store
export default observer(App);
