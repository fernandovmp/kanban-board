import React from 'react';
import { IOverlayProps } from '../Overlay';
import { ModalCard, ModalPanel } from './styles';

export { ModalPanel };

interface IModalProps extends IOverlayProps {}

export const Modal: React.FC<IModalProps> = ({
    showCloseButton,
    title,
    children,
    onClose,
}) => {
    return (
        <ModalPanel>
            <ModalCard
                onClose={onClose}
                showCloseButton={showCloseButton}
                title={title}
            >
                {children}
            </ModalCard>
        </ModalPanel>
    );
};
