import React from 'react';
import { Link, Route, useHistory } from 'react-router-dom';
import exitIcon from '../../assets/exit_to_app.svg';
import homeIcon from '../../assets/home.svg';
import { setJwtToken } from '../../services/tokenService';
import { AppBarHeader, AppName, Icon, ToolBar } from './styles';

export const AppBar: React.FC = () => {
    const history = useHistory();
    const handleLogOut = () => {
        setJwtToken('');
        history.push('/login');
    };

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
