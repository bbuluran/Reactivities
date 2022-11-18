import React, { Fragment } from 'react';
import { Container } from 'semantic-ui-react';
import NavBar from './NavBar';
import { observer } from 'mobx-react-lite';
import { Route, Switch, useLocation } from 'react-router-dom';
import HomePage from '../../features/home/HomePage';
import ActivityDashboard from '../../features/activities/dashboard/ActivityDashboard';
import ActivityForm from '../../features/activities/form/ActivityForm';
import ActivityDetails from '../../features/activities/details/ActivityDetails';
import TestError from '../../features/errors/TestError';
import { ToastContainer } from 'react-toastify';
import NotFound from '../../features/errors/NotFound';
import ServerError from '../../features/errors/ServerError';

function App() {
  // declare access modifier 'const'
  // [activities, setActivities] -> activities = variable where state will be stored, 
  //                             -> setActivities = function we use to set state
  // useState([]) -> [] = declare initial state of 'activities' variable

  // REACT hooks -> useState allow us to store state in component
  // REACT hooks -> useEffect allow us to create side effects when a component initializes
  // ex.   const [activities, setActivities] = useState<Activity[]>([]);

  const location = useLocation();

  return (
    <Fragment>
      <ToastContainer position='bottom-right' hideProgressBar />
      <Route exact path='/' component={HomePage}/>
      <Route
        path={'/(.+)'}
        render={() => (
          <Fragment>
            <NavBar />
            <Container style={{marginTop: '7em'}}>
              <Switch>
                <Route exact path='/activities' component={ActivityDashboard}/>
                <Route path={['/createActivity', '/manage/:id']} component={ActivityForm} key={location.key}/>
                <Route path='/activities/:id' component={ActivityDetails} />
                <Route path='/errors' component={TestError} />
                <Route path='/server-error' component={ServerError} />
                <Route component={NotFound} />
              </Switch>
            </Container>
          </Fragment>
        )}
      />
    </Fragment>
  );
}

// if we want to make our components observe changes in our store,
// we need to wrap the 'App' function to the observer.
// This observer higher order function returns the 'App' function,
// with additional powers to observe the store
export default observer(App);
