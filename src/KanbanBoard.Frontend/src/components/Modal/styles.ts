import styled from 'styled-components';
import { Card } from '..';

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

export const ModalCard = styled(Card)`
    display: flex;
    flex-direction: column;
    background: white;
    padding: 12px;
`;

export const Header = styled.header`
    font-weight: bold;
    display: flex;
    flex-direction: column;
    justify-content: flex-end;
`;
export const Main = styled.main`
    margin-top: 10px;
`;
export const Separator = styled.div`
    background: var(--text);
    height: 1px;
    width: 100%;
    margin: 0;
`;
