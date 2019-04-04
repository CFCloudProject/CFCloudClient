﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFCloudClient.FileUtil
{
    public class CDC
    {
        readonly ulong[] out_table =
        {
            0, 6732594018357782, 5195956652795231, 1582145096087369,
            7116410070240205, 4119957657983451, 3164290192174738, 8117305825262724,
            4278857543571177, 6993834903618815, 8239915315966902, 3005424764887456,
            6328580384349476, 440411371622194, 1141836906444923, 5600073334565485,
            8557715087142354, 2687689555591108, 3398056277264525, 7874434173295259,
            2022431808792095, 4719267843552265, 6010849529774912, 758215371706710,
            4878169768622907, 1899858832648493, 880822743244388, 5851981865880690,
            2283673812889846, 8998124199076576, 7434128037811625, 3802175130634175,
            401669988457175, 6339866289440961, 5375379111182216, 1393780382363038,
            6796112554529050, 4448922282689292, 2903495269446725, 8369433634793043,
            4044863617584190, 7236478760836648, 7929101756770657, 3307588397054839,
            6498794024159219, 279122805183973, 1516430743413420, 5216554420023482,
            8737146124973829, 2499333438088467, 3799717665296986, 7481697860730956,
            1761645486488776, 4971404236730078, 5690543434619287, 1087171400190849,
            4567347625779692, 2202013864391674, 646837413479603, 6094634302543525,
            2658259053636129, 8614596705089591, 7604350261268350, 3640895164620136,
            803339976914350, 5965813107697592, 4696099113390321, 2045443640541927,
            7761403182399075, 3511523607882869, 2787560764726076, 8457476253567274,
            3915614546092871, 7321069131755857, 8897844565378584, 2383504221901838,
            5806990538893450, 925992443542172, 1922894284590549, 4855024658653123,
            8089727235168380, 3191686109792874, 4160696025644323, 7075785651384117,
            1677606449484721, 5100101909283239, 6615176794109678, 117879338034424,
            5504194966670997, 1237274635459715, 558245610367946, 6211118060812764,
            3032861486826840, 8212377530193742, 6953238404112391, 4319623830581777,
            1046680259506041, 5731415080451439, 4998666876176934, 1733933618223152,
            7599435330593972, 3682158572877474, 2403620562747883, 8832749346542589,
            3523290972977552, 7722042631908230, 8710176302210255, 2562522553847513,
            6135533867790941, 606374192209995, 2174342800381698, 4594651069482260,
            8392303598371499, 2880184671113405, 4404027728783348, 6841379027908066,
            1293674826959206, 5475383598488432, 6453200341888057, 288505719389743,
            5316518107272258, 1416284387952212, 165930620891421, 6612100161381131,
            3284301418979215, 7951995340606873, 7281790329240272, 4000014158702790,
            1606679953828700, 5169931071781194, 6686086141349379, 48067386874901,
            8159553195237521, 3120758525784711, 4090887281083854, 7146696091178968,
            2963035459665333, 8283305181327267, 7023047215765738, 4248713323662076,
            5575121529452152, 1167445405869166, 487336196002599, 6280930079064369,
            7831229092185742, 3440596007015576, 2717752003339217, 8528386643112391,
            732413430975811, 6035642253336405, 4767008443803676, 1975631639132682,
            5877917151924327, 856163230777969, 1851984887084344, 4924836727187246,
            3845788569181098, 7391996799715772, 8967653393891061, 2312593765264611,
            1363485805048203, 5404475291427741, 6383372219585748, 359431241959106,
            8321392051288646, 2949994590070864, 4474956487204633, 6771551960534287,
            3355212898969442, 7882185488774516, 7210861637911101, 4069841158950955,
            5246707196308655, 1487192627920569, 235758676068848, 6541174705904614,
            7528523766651993, 3751968441552463, 2474549270919430, 8762922262342416,
            1116491220735892, 5660506723108226, 4928838703624907, 1804859123966173,
            6065722973653680, 677282482428070, 2244170905841647, 4523725630864889,
            3594202469794173, 7652232830325611, 8639247661163554, 2632349570954804,
            2093360519012082, 4649440761531620, 5939937967979437, 828025242397115,
            8487886912443711, 2758615059318569, 3467867236446304, 7803525813772918,
            2353501920462875, 8927198762441229, 7364317145754948, 3873083423063890,
            4807241125495766, 1969685847544256, 951734237947529, 5782172062315679,
            7046581945955104, 4190883178438966, 3234101168051839, 8046397516121193,
            70928760633581, 6662766953065211, 5125045107695026, 1651955017158564,
            6257651690841545, 510238369822687, 1212748384419990, 5530263480586880,
            4348685600763396, 6922909450288146, 8170104407182171, 3076333006903629,
            1691836723086373, 5042314624128563, 5760369342226810, 1016243797275500,
            8808055457566696, 2429521438727678, 3728791121373879, 7551527008516257,
            2587349653918412, 8684408771543258, 7675276872316819, 3571065949742469,
            4637156456274177, 2131103409867543, 577011438779486, 6165561972584008,
            6867021903849975, 4379110333742049, 2832568775904424, 8439262799273662,
            331861241782842, 6410776727253036, 5445205069170533, 1322852796142963,
            6568602837958430, 208212300279048, 1446604718299713, 5287482073335895,
            3973954201171155, 7306290776909509, 8000028317405580, 3237759165449114
        };

        readonly ulong[] mod_table =
        {
            0, 17349423945073011, 19955442907497365, 34698847890146022,
            39910885814994730, 49655631705097817, 57192075069408447, 69397695780292044,
            79821771629989460, 82813360312471335, 98358070728412609, 99311263410195634,
            114384150138816894, 120467934718478349, 130809558995290859, 138795391560584088,
            147081401575923163, 159643543259978920, 165626720624942670, 176150191431507773,
            182188511369321201, 196716141456825218, 198622526820391268, 215052479770449943,
            220989446515250063, 228768300277633788, 240935869436956698, 246108979115938153,
            260318200427336869, 261619117990581718, 277590783121168176, 281352300626302531,
            294162803151846326, 299683676331080389, 311864225417525283, 319287086519957840,
            331253441249885340, 334807932742860271, 350784210128437001, 352300382863015546,
            364377022738642402, 374684757338584209, 380663733689579127, 393432282913650436,
            397245053640782536, 414031480286081979, 415924474481504605, 430104959540899886,
            441240903978346093, 441978893030500126, 457536600555267576, 460735129352660107,
            473529965190373703, 481871738873913396, 492217958231876306, 497954065208533921,
            505545799306405945, 520636400854673738, 523238235981163436, 540231100329552607,
            543182404608978707, 555181566242336352, 562704601252605062, 572665169250774517,
            583080018942539295, 588325606303692652, 599367352662160778, 607073746056459513,
            619894266870509877, 623728450835050566, 638574173039915680, 639802406829667283,
            651915057397741643, 662506882499770680, 669615865485720542, 682109669919963821,
            685070473587277665, 701568420256874002, 704600765726031092, 719060385048345991,
            728754045477284804, 729775300530973367, 746446005412243537, 749369514677168418,
            761327467379158254, 769381585587547549, 780849049845084027, 786864565827300872,
            794490107281565072, 809306247510205667, 810786289187169797, 828062960572163958,
            831848948963009210, 844126978569479113, 850537564773196079, 860209919081799772,
            865420872780680617, 882481807956692186, 883957786061000252, 898980333827297103,
            905044373104855683, 915073201110535152, 921470258705320214, 933401143285516389,
            943788657613210621, 947059930380747406, 963743477747826792, 964408740143923483,
            978627426495530199, 984435916463752612, 995908130417067842, 1004177503794955825,
            1011091598612811890, 1023932599615033601, 1031037536201642983, 1041272801709347476,
            1046476471962326872, 1060729632205280811, 1063748431883277517, 1080462200659105214,
            1086364809217957414, 1093855998980267861, 1104910605010967987, 1110363132484672704,
            1125409202505210124, 1126993377415043199, 1141843832138177177, 1145330338501549034,
            1158176438737218893, 1166160037885078590, 1176651212607385304, 1182737230468427691,
            1197783677271288423, 1198734705324321556, 1214147492112919026, 1217141245560727681,
            1227580748332114713, 1239788533741019754, 1247456901670101132, 1257199482998235647,
            1262402707748889651, 1277148346079831360, 1279604813659334566, 1296952004119537365,
            1303830114795483286, 1307593796864356837, 1323715012132418307, 1325013764999541360,
            1339231730971441084, 1344407074132866767, 1356442719428129833, 1364219339839927642,
            1370140947174555330, 1386568666841333681, 1388606977010580823, 1403136840513748004,
            1409201531452062184, 1419722837495593115, 1425556463781460605, 1438120770096691982,
            1443325441197151995, 1457508090954569608, 1459550601061946734, 1476334863145276445,
            1480121228252123601, 1492892010824487074, 1498739029354336836, 1509044530469409591,
            1521140995441170607, 1522654934758316508, 1538763171175095098, 1542319895949449801,
            1554277403216486277, 1561698099690168054, 1573729131654601744, 1579252169599163747,
            1588980214563130144, 1598938549278018131, 1606611099971389621, 1618612495020411334,
            1621572578374339594, 1638563277959695737, 1641033154964885407, 1656125921144327916,
            1663697897926018420, 1669436169466414087, 1679914348151539425, 1688253957138958226,
            1701075129546392158, 1704275891826230061, 1719684082462032331, 1720419838163599544,
            1730841745561361234, 1745299131466242593, 1748463435962407111, 1764963615913384372,
            1767915572122000504, 1780407211927493899, 1787366677787238381, 1797960667654594206,
            1810088746209711366, 1811319144697484405, 1826314382818498195, 1830146402221070304,
            1842940517410640428, 1850649144153232223, 1861558932694749625, 1866802286571032778,
            1877902796354078345, 1887577315226421242, 1894119860761494812, 1906395725671842927,
            1910208050628215203, 1927486955495653584, 1928817480287846966, 1943631387165901637,
            1951241570292123869, 1957254852991060398, 1968871832927505224, 1976928184551557691,
            1988894916332244983, 1991816260834135684, 2008355007589911650, 2009378427274775825,
            2022183197225623780, 2030454735301534103, 2042058873823814513, 2047865199230067202,
            2062075072403285966, 2062742568147674813, 2079276564136027227, 2082545603418694952,
            2092952943924653744, 2104881595087417283, 2111428203123502373, 2121459264410561622,
            2127496863766555034, 2142517246904101097, 2143861301376870927, 2160924401318210428,
            2169245345355824447, 2172729618435914828, 2187711997960535722, 2189298406286032857,
            2204370847311264277, 2209821210021935974, 2220726264969345408, 2228219619362830579,
            2234102471670854507, 2250818405010420248, 2253986754830086398, 2268237750376919437,
            2273450165286204481, 2283687664276354354, 2290660677003098068, 2303499444654732967
        };

        const ulong POLYNOMIAL = 0x3DA3358B4DC173;
        const int POLYNOMIAL_DEGREE = 53;
        const int WINSIZE = 64;
        const int AVERAGE_BITS = 13;
        const int MINSIZE = 1024;
        const int MAXSIZE = 32 * 1024;

        const int MASK = ((1 << AVERAGE_BITS) - 1);
        const int POL_SHIFT = POLYNOMIAL_DEGREE - 8;

        byte[] window = new byte[WINSIZE];
        uint wpos;
        uint count;
        uint pos;
        uint start;
        ulong digest;

        bool tables_initialized = false;
        //ulong[] mod_table = new ulong[256];
        //ulong[] out_table = new ulong[256];

        public Chunk last_chunk = new Chunk();

        public void rabin_init()
        {
            /*if (!tables_initialized)
            {
                //calc_tables();
                for (int i = 0; i < 256; i++)
                {
                    mod_table[i] = MOD_TABLE[i];
                    out_table[i] = OUT_TABLE[i];
                }
                tables_initialized = true;
            }*/

            rabin_reset();
        }

        private void rabin_reset()
        {
            for (int i = 0; i < WINSIZE; i++)
                window[i] = 0;
            wpos = 0;
            count = 0;
            digest = 0;

            rabin_slide(1);
        }

        private void rabin_slide(byte b)
        {
            byte o = window[wpos];
            window[wpos] = b;
            digest = (digest ^ out_table[o]);
            wpos = (wpos + 1) % WINSIZE;
            rabin_append(b);
        }

        private void rabin_append(byte b)
        {
            byte index = (byte)(digest >> POL_SHIFT);
            digest <<= 8;
            digest |= (ulong)b;
            digest ^= mod_table[index];
        }

        public int rabin_next_chunk(byte[] buf, int start_index, int len)
        {
            for (uint i = 0; i < len; i++)
            {
                byte b = buf[i + start_index];

                rabin_slide(b);

                count++;
                pos++;

                if ((count >= MINSIZE && ((digest & MASK) == 0)) || count >= MAXSIZE)
                {
                    last_chunk.start = start;
                    last_chunk.length = count;
                    last_chunk.cut_fingerprint = digest;

                    // keep position
                    uint position = pos;
                    rabin_reset();
                    start = position;
                    pos = position;

                    return (int)i + 1;
                }
            }

            return -1;
        }

        public Chunk rabin_finalize()
        {
            if (count == 0)
            {
                last_chunk.start = 0;
                last_chunk.length = 0;
                last_chunk.cut_fingerprint = 0;
                return null;
            }

            last_chunk.start = start;
            last_chunk.length = count;
            last_chunk.cut_fingerprint = digest;
            return last_chunk;
        }

        private int deg(ulong p)
        {
            ulong mask = 0x8000000000000000;

            for (int i = 0; i < 64; i++)
            {
                if ((mask & p) > 0)
                {
                    return 63 - i;
                }

                mask >>= 1;
            }

            return -1;
        }

        private ulong mod(ulong x, ulong p)
        {
            while (deg(x) >= deg(p))
            {
                int shift = deg(x) - deg(p);
                x = x ^ (p << shift);
            }

            return x;
        }

        private ulong append_byte(ulong hash, byte b, ulong pol)
        {
            hash <<= 8;
            hash |= (ulong)b;

            return mod(hash, pol);
        }

        private void calc_tables()
        {
            // calculate table for sliding out bytes. The byte to slide out is used as
            // the index for the table, the value contains the following:
            // out_table[b] = Hash(b || 0 ||        ...        || 0)
            //                          \ windowsize-1 zero bytes /
            // To slide out byte b_0 for window size w with known hash
            // H := H(b_0 || ... || b_w), it is sufficient to add out_table[b_0]:
            //    H(b_0 || ... || b_w) + H(b_0 || 0 || ... || 0)
            //  = H(b_0 + b_0 || b_1 + 0 || ... || b_w + 0)
            //  = H(    0     || b_1 || ...     || b_w)
            //
            // Afterwards a new byte can be shifted in.
            for (int b = 0; b < 256; b++)
            {
                ulong hash = 0;

                hash = append_byte(hash, (byte)b, POLYNOMIAL);
                for (int i = 0; i < WINSIZE - 1; i++)
                {
                    hash = append_byte(hash, 0, POLYNOMIAL);
                }
                out_table[b] = hash;
            }

            // calculate table for reduction mod Polynomial
            int k = deg(POLYNOMIAL);
            for (int b = 0; b < 256; b++)
            {
                // mod_table[b] = A | B, where A = (b(x) * x^k mod pol) and  B = b(x) * x^k
                //
                // The 8 bits above deg(Polynomial) determine what happens next and so
                // these bits are used as a lookup to this table. The value is split in
                // two parts: Part A contains the result of the modulus operation, part
                // B is used to cancel out the 8 top bits so that one XOR operation is
                // enough to reduce modulo Polynomial
                mod_table[b] = mod(((ulong)b) << k, POLYNOMIAL) | ((ulong)b) << k;
            }
        }
    }
}
