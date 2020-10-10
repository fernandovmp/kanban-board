import React from 'react';
import { Route, Switch } from 'react-router-dom';
import { BoardPage, UserBoards } from '..';
import { AppBar } from '../../components';
import { TemplateContent } from './styles';

export const AppTemplate: React.FC = () => {
    return (
        <TemplateContent>
            <AppBar />
            <Switch>
                <Route path="/board/:boardId" component={BoardPage} />
                <Route path="/" component={UserBoards} />
            </Switch>
        </TemplateContent>
    );
};
