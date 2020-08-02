import React from 'react';
import { BrowserRouter, Route, Switch } from 'react-router-dom';
import { AppTemplate } from './pages';

export const Routes: React.FC = () => {
    return (
        <BrowserRouter>
            <Switch>
                <Route path="/" component={AppTemplate} />
            </Switch>
        </BrowserRouter>
    );
};
