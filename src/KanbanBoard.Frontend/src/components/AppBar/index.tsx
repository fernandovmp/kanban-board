import React from 'react';
import { Link, Route } from 'react-router-dom';
import exitIcon from '../../assets/exit_to_app.svg';
import homeIcon from '../../assets/home.svg';
import { AppBarHeader, AppName, Icon, ToolBar } from './styles';

export const AppBar: React.FC = () => {
    const handleLogOut = () => {};

    return (
        <AppBarHeader>
            <ToolBar align="left">
                <Route exact path="/(\S+)">
                    <Link to="/">
                        <Icon src={homeIcon} alt="Home" />
                    </Link>
                </Route>
            </ToolBar>
            <AppName>Kanban Board</AppName>
            <ToolBar align="right">
                <Icon src={exitIcon} alt="Log Out" onClick={handleLogOut} />
            </ToolBar>
        </AppBarHeader>
    );
};
