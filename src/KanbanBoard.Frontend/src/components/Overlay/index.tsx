import React from 'react';
import { Header, Main, OverlayWrapper, Separator } from './styles';

interface IOverlayProps {
    title: JSX.Element | string;
}

export const Overlay: React.FC<IOverlayProps> = ({ title, children }) => {
    return (
        <OverlayWrapper>
            <Header>
                {title}
                <Separator />
            </Header>
            <Main>{children}</Main>
        </OverlayWrapper>
    );
};
