import styled from 'styled-components';
import { DefaultButton, IconButton, Overlay } from '../../../../components';

export const ListOptionsOverlay = styled(Overlay)`
    position: absolute;
    top: -12px;
    left: 16px;
    width: 200px;
`;

export const RelativeOverlay = styled.div`
    position: relative;
`;

export const Header = styled.header`
    display: flex;
    position: relative;
    align-items: center;
    justify-content: center;
    width: 100%;
`;

export const DeleteButton = styled(DefaultButton)`
    background-color: #ff3131;
    color: white;
    width: 100%;
    display: flex;
    justify-content: center;
`;

export const CloseButton = styled(IconButton)`
    position: absolute;
    right: 0;
`;
