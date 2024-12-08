import { UseQueryResult } from '@tanstack/react-query'
import { PaginationState, SortingState, flexRender, getCoreRowModel, getFilteredRowModel, getPaginationRowModel, getSortedRowModel, useReactTable } from '@tanstack/react-table'
import { useEffect, useState } from 'react'
import TableError from './TableError';
import TableLoading from '../Loading';

type Props = {
    tableDefinition: any[]
    query: UseQueryResult
    searchable?: boolean,
    pageSize?: number,
    className?: string,
}

function DataTable({ tableDefinition, query, searchable=true, pageSize=10, className=''} : Props) {
    const [sorting, setSorting] = useState<SortingState>([])
    const [pagination, setPagination] = useState<PaginationState>({pageIndex: 0, pageSize: pageSize })
    const [filtering, setFiltering] = useState('')
    const [data, setData]= useState<typeof query.data>([])

    const table = useReactTable({
        columns: tableDefinition,
        data: data as unknown[],
        getCoreRowModel: getCoreRowModel(),
        getSortedRowModel: getSortedRowModel(),
        getFilteredRowModel: getFilteredRowModel(),
        getPaginationRowModel: getPaginationRowModel(),
        state: {
            pagination: pagination,
            sorting: sorting,
            globalFilter: filtering,
        },
        onPaginationChange: setPagination,
        onSortingChange: setSorting,
        onGlobalFilterChange: setFiltering
    })

    useEffect(() => {
        setData(query.data || []);
    }, [query.data, setData]);

    return(
        <div className={`${className}`}>
            {searchable &&
            <div className='flex justify-end mb-2'>
                <div className='relative max-w-64'>
                    <span className="text-sm top-3.5 left-3 absolute flex text-slate-500">
                        <i className="fas fa-search" aria-hidden="true"></i>
                    </span>
                    <input className="pl-9 w-full primary" type="text" placeholder="Rechercher" value={filtering} onChange={e => setFiltering(e.target.value)} />
                </div>
            </div>}
            <div className='block min-w-full divide-y divide-gray-200 overflow-auto'>
                <table className="w-full max-w-full mb-4">
                    <thead>
                        {table.getHeaderGroups().map((headerGroup) => (
                            <tr key={headerGroup.id}>
                                {headerGroup.headers.map((header) => (
                                    <th key={header.id} onClick={header.column.getToggleSortingHandler()} className='text-md px-2 py-2 text-sm uppercase'>
                                        <div className="flex justify-center">
                                            {{
                                                asc: <span className="rotate-180 w-[15px] me-1 opacity-100">
                                                        <svg xmlns="http://www.w3.org/2000/svg" fill="currentColor" viewBox="0 0 24 24" strokeWidth="3" stroke="currentColor">
                                                            <path strokeLinecap="round" strokeLinejoin="round" d="M4.5 10.5L12 3m0 0l7.5 7.5M12 3v18"></path>
                                                        </svg>
                                                    </span>,
                                                desc: <span className="rotate-0 w-[15px] me-1 opacity-100">
                                                        <svg xmlns="http://www.w3.org/2000/svg" fill="currentColor" viewBox="0 0 24 24" strokeWidth="3" stroke="currentColor">
                                                            <path strokeLinecap="round" strokeLinejoin="round" d="M4.5 10.5L12 3m0 0l7.5 7.5M12 3v18"></path>
                                                        </svg>
                                                    </span>
                                            }[header.column.getIsSorted() as string] ?? null}
                                            {flexRender(header.column.columnDef.header, header.getContext() )}
                                        </div>
                                    </th>
                                ))}
                            </tr>
                        ))}
                    </thead>
                    <tbody className='text-gray-600'>
                    {
                    (query.isError)?
                        <tr><td colSpan={table.getAllColumns().length}>
                            <TableError msg={(query.error as any).response.data?.errorMessage} />    
                        </td></tr>:

                    query.isLoading?
                        <tr><td colSpan={table.getAllColumns().length}><TableLoading /></td></tr>:

                    table.getRowModel().rows?.length == 0 ?
                    <tr><td colSpan={table.getHeaderGroups()[0].headers.length} className="py-2 text-center"> Pas de lignes à afficher </td></tr> :
                    table.getRowModel().rows?.map((row) => (
                        <tr key={row.id}>
                            {row.getVisibleCells().map(cell => (
                                <td key={cell.id} className='py-2 px-2'>
                                    {flexRender(cell.column.columnDef.cell, cell.getContext())}
                                </td>
                            ))}
                        </tr>)
                    )
                    }
                    </tbody>
                </table>
            </div>
            <div className='flex justify-center gap-6'>
                <button className="flex justify-center w-6 items-center rounded-full text-gray-700 disabled:text-gray-400 hover:bg-gray-50" disabled={!table.getCanPreviousPage()} onClick={() => table.previousPage()}>
                    <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth="2" stroke="currentColor" aria-hidden="true" className="w-4 h-4">
                        <path strokeLinecap="round" strokeLinejoin="round" d="M10.5 19.5L3 12m0 0l7.5-7.5M3 12h18"></path>
                    </svg>
                </button>
                <div className='flex items-center gap-1'>
                    <div>Page</div>
                    <strong> {table.getState().pagination.pageIndex + 1} sur {table.getPageCount().toLocaleString()} </strong>
                </div>
                <button className="flex justify-center w-6 items-center rounded-full text-gray-700 disabled:text-gray-400 hover:bg-gray-50" disabled={!table.getCanNextPage()} onClick={() => table.nextPage()}>
                    <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth="2" stroke="currentColor" aria-hidden="true" className="w-4 h-4">
                        <path strokeLinecap="round" strokeLinejoin="round" d="M13.5 4.5L21 12m0 0l-7.5 7.5M21 12H3"></path>
                    </svg>
                </button>
            </div>
        </div>
    )
}

export default DataTable;