import React from 'react';
import closeIcon from '../../assets/close.svg';
import { CloseButton, Header, Main, OverlayWrapper, Separator } from './styles';

export interface IOverlayProps {
    onClose?: () => void;
    showCloseButton?: boolean;
    title: JSX.Element | string;
}

export const Overlay: React.FC<IOverlayProps> = ({
    title,
    children,
    onClose,
    showCloseButton,
}) => {
    return (
        <OverlayWrapper>
            <Header>
                {showCloseButton && (
                    <CloseButton
                        src={closeIcon}
                        alt="Close"
                        onClick={onClose}
                    />
                )}
                {title}
                <Separator />
            </Header>
            <Main>{children}</Main>
        </OverlayWrapper>
    );
};
