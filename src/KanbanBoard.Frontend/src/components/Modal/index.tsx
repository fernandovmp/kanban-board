import React from 'react';
import { Header, Main, ModalCard, ModalPanel, Separator } from './styles';

export { ModalPanel };

interface IModalProps {
    title: JSX.Element | string;
}

export const Modal: React.FC<IModalProps> = ({ title, children }) => {
    return (
        <ModalPanel>
            <ModalCard>
                <Header>
                    {title}
                    <Separator />
                </Header>
                <Main>{children}</Main>
            </ModalCard>
        </ModalPanel>
    );
};
