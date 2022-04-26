using System;
using System.Collections.Generic;
namespace ImageProcessing
{
    public static class InformationTables
    {


        // https://www.thonky.com/qr-code-tutorial/character-capacities
        public static Dictionary<int, Dictionary<char, Dictionary<byte, int>>> Capacities =
            new Dictionary<int, Dictionary<char, Dictionary<byte, int>>>
            {
                {
                    1,
                    new Dictionary<char, Dictionary<byte, int>>
                    {
                        {
                            'L',
                            new Dictionary<byte, int>
                            {
                                {
                                    0b0010,
                                    25
                                }
                            }
                        }
                    }
                },

                {
                    2,
                    new Dictionary<char, Dictionary<byte, int>>
                    {
                        {
                            'L',
                            new Dictionary<byte, int>
                            {
                                {
                                    0b0010,
                                    47
                                }
                            }
                        }
                    }
                },

                {
                    3,
                    new Dictionary<char, Dictionary<byte, int>>
                    {
                        {
                            'L',
                            new Dictionary<byte, int>
                            {
                                {
                                    0b0010,
                                    77
                                }
                            }
                        }
                    }
                },

                {
                    4,
                    new Dictionary<char, Dictionary<byte, int>>
                    {
                        {
                            'L',
                            new Dictionary<byte, int>
                            {
                                {
                                    0b0010,
                                    114
                                }
                            }
                        }
                    }
                },

                {
                    5,
                    new Dictionary<char, Dictionary<byte, int>>
                    {
                        {
                            'L',
                            new Dictionary<byte, int>
                            {
                                {
                                    0b0010,
                                    154
                                }
                            }
                        }
                    }
                },

                {
                    6,
                    new Dictionary<char, Dictionary<byte, int>>
                    {
                        {
                            'L',
                            new Dictionary<byte, int>
                            {
                                {
                                    0b0010,
                                    195
                                }
                            }
                        }
                    }
                },

                {
                    7,
                    new Dictionary<char, Dictionary<byte, int>>
                    {
                        {
                            'L',
                            new Dictionary<byte, int>
                            {
                                {
                                    0b0010,
                                    224
                                }
                            }
                        }
                    }
                }
            };

        // https://www.thonky.com/qr-code-tutorial/alignment-pattern-locations
        public static Dictionary<int, int[]> AlignmentPatternLocations =
            new Dictionary<int, int[]>
            {
                {
                    1,
                    new int[] {}
                },

                {
                    2,
                    new int[] {6, 18}
                },

                {
                    3,
                    new int[] {6, 22}
                },

                {
                    4,
                    new int[] {6, 26}
                },

                {
                    5,
                    new int[] {6, 30}
                },

                {
                    6,
                    new int[] {6, 34}
                },

                {
                    7,
                    new int[] {6, 22, 38}
                }
            };


        // https://www.thonky.com/qr-code-tutorial/error-correction-table
        public static Dictionary<int, int> ErrorCorrectionBytes =
            new Dictionary<int, int>
            {
                {
                    1,
                    7
                },

                {
                    2,
                    10
                },

                {
                    3,
                    15
                },

                {
                    4,
                    20
                },

                {
                    5,
                    26
                },

                {
                    6,
                    18
                },

                {
                    7,
                    20
                }
            };

    }
}
