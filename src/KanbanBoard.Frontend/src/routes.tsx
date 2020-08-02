import React from 'react';
import { BrowserRouter, Route, Switch } from 'react-router-dom';
import { AppTemplate, AuthPage } from './pages';

export const Routes: React.FC = () => {
    return (
        <BrowserRouter>
            <Switch>
                <Route exact path="\/(login|signup)" component={AuthPage} />
                <Route path="/" component={AppTemplate} />
            </Switch>
        </BrowserRouter>
    );
};
