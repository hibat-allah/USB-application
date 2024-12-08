import Alert from "../Alert";
type Props = {
    msg?: string
}

function TableError({ msg }: Props) {
    return <Alert color='bg-red-400' className='text-center absolute !mb-0'> {msg?? "Échec Réseau - Service injoignable en ce moment. Veuillez réessayer ultérieurement."} </Alert>;
}
export default TableError;