import React from 'react';
import './App.css';
import Home from './components/Home';
import {
  BrowserRouter as Router,
  Switch,
  Route,
  Redirect,
} from "react-router-dom";
import Header from './components/Header';
import Viewers from './components/Viewers';

function App() {
  return (
    <div className="App">
      <Router>
        <Header />
        <Switch>
          <Route exact path="/">
            <Home />
          </Route>
          <Route path="/process">
            <Viewers />
          </Route>
        </Switch>
      </Router>
    </div>
  );
}

export default App;
