import React from 'react';
import { BrowserRouter, Route, Switch } from 'react-router-dom';
import { BoardPage, UserBoards } from './pages';

export const Routes: React.FC = () => {
    return (
        <BrowserRouter>
            <Switch>
                <Route path="/board/:boardId" component={BoardPage} />
                <Route path="/" component={UserBoards} />
            </Switch>
        </BrowserRouter>
    );
};
