import styled from 'styled-components';
import { Overlay } from '../Overlay';

export const ModalPanel = styled.div`
    position: fixed;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
    background: rgba(0, 0, 0, 0.2);
    display: flex;
    align-items: center;
    justify-content: center;
    z-index: 5;
`;

export const ModalCard = styled(Overlay)`
    position: unset;
    flex-direction: column;
`;
