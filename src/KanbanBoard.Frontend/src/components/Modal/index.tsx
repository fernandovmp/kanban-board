import React from 'react';
import { ModalCard, ModalPanel } from './styles';

export { ModalPanel };

interface IModalProps {
    title: JSX.Element | string;
}

export const Modal: React.FC<IModalProps> = ({ title, children }) => {
    return (
        <ModalPanel>
            <ModalCard title={title}>{children}</ModalCard>
        </ModalPanel>
    );
};
