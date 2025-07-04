import { NextRequest, NextResponse } from 'next/server';
import { getCurrentUser } from '@/lib/actions/auth-actions';
import { getAllWishlists } from '@/lib/actions/admin-wishlist-actions';

export async function GET(request: NextRequest) {
  try {
    // Check admin authentication
    const user = await getCurrentUser();
    if (!user) {
      return NextResponse.json({ error: 'Unauthorized' }, { status: 401 });
    }

    // Add role checking here if needed
    // if (!user.roles?.includes('Admin')) {
    //   return NextResponse.json({ error: 'Forbidden' }, { status: 403 });
    // }

    const { searchParams } = new URL(request.url);
    const format = searchParams.get('format') || 'json';

    // Get all wishlists
    const wishlists = await getAllWishlists();

    switch (format.toLowerCase()) {
      case 'json':
        return NextResponse.json(wishlists, {
          headers: {
            'Content-Disposition': `attachment; filename="wishlists-${new Date().toISOString().split('T')[0]}.json"`,
            'Content-Type': 'application/json'
          }
        });

      case 'csv':
        const csvHeaders = 'ID,Name,User ID,Is Default,Is Public,Item Count,Total Value,Created Date,Updated Date\n';
        const csvData = wishlists.map(w => 
          `${w.id},"${w.name}",${w.userId},${w.isDefault},${w.isPublic},${w.itemCount},${w.totalValue},"${w.createdDate}","${w.updatedDate}"`
        ).join('\n');
        
        return new NextResponse(csvHeaders + csvData, {
          headers: {
            'Content-Disposition': `attachment; filename="wishlists-${new Date().toISOString().split('T')[0]}.csv"`,
            'Content-Type': 'text/csv'
          }
        });

      case 'xlsx':
        // For XLSX format, you'd typically use a library like 'xlsx'
        // For now, we'll return JSON with appropriate headers
        return NextResponse.json(wishlists, {
          headers: {
            'Content-Disposition': `attachment; filename="wishlists-${new Date().toISOString().split('T')[0]}.json"`,
            'Content-Type': 'application/json'
          }
        });

      default:
        return NextResponse.json({ error: 'Unsupported format' }, { status: 400 });
    }
  } catch (error) {
    console.error('Export error:', error);
    return NextResponse.json({ error: 'Export failed' }, { status: 500 });
  }
} 