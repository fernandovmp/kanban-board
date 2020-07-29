import React from 'react';
import exitIcon from '../../assets/exit_to_app.svg';
import { AppBarHeader, AppName, LogOutIcon, ToolBar } from './styles';

export const AppBar: React.FC = () => {
    const handleLogOut = () => {};

    return (
        <AppBarHeader>
            <ToolBar />
            <AppName>Kanban Board</AppName>
            <ToolBar>
                <LogOutIcon
                    src={exitIcon}
                    alt="Log Out"
                    onClick={handleLogOut}
                />
            </ToolBar>
        </AppBarHeader>
    );
};
